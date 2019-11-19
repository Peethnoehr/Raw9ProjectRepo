-- GROUP: raw9, MEMBERS: Baronnet, Aurelien; Føhns-Greve, Marco; Klitgaard, Christian; Nøhr, Peter
/* D1 */ 
CREATE OR REPLACE FUNCTION searchString ( string VARCHAR ( 255 ), userid INTEGER ) RETURNS TABLE ( postid INTEGER, title TEXT, body TEXT ) AS $$ BEGIN
	RETURN query (
		SELECT DISTINCT
			QA_Post.postid,
			QA_Question.title,
			QA_Post.body 
		FROM
			QA_Post
			LEFT JOIN QA_Question ON QA_Post.postid = QA_Question.questionid 
		WHERE
			LOWER ( QA_Question.title ) LIKE LOWER ( concat ( '%', string, '%' ) ) 
			OR LOWER ( QA_Post.body ) LIKE LOWER ( concat ( '%', string, '%' ) ) 
		);
	INSERT INTO HM_SearchHistory ( searchdate, searchtext, userid )
	VALUES
		( NOW( ), string, userid );
	
END;
$$ LANGUAGE plpgsql;

/* D2 */
DROP TABLE
IF
	EXISTS wi;
CREATE TABLE wi AS SELECT ID
,
LOWER ( word ) word 
FROM
	words 
WHERE
	word ~* '^[a-z][a-z0-9_]+$' 
	AND tablename = 'posts' 
	AND (what = 'body' OR what='title') 
GROUP BY
	ID,
	word;
	
/* D3 */
CREATE OR REPLACE FUNCTION exactMatchQuery (string VARCHAR(255)) RETURNS TABLE ( postid INTEGER, body TEXT, title TEXT ) AS $$ 
DECLARE 
	q TEXT := '';
	string_elem TEXT;
	i INTEGER := 1;
BEGIN
	q := 'SELECT postid, body, title FROM QA_Post LEFT JOIN QA_Question ON QA_Post.postid = QA_Question.questionid ,(';
	FOR string_elem IN SELECT regexp_split_to_table(string, ' ')
	LOOP 
		q := q || 'SELECT id FROM wi WHERE word = ''';
		q := q || string_elem;
		IF (SELECT count(*) FROM regexp_split_to_table(string, ' ')) <> i THEN
			q := q || ''' INTERSECT ';
		END IF;
		i := i+1;
	END LOOP;
	q := q || ''') t WHERE id = postid;';
	RAISE NOTICE '%', q;
	RETURN QUERY EXECUTE q;
END;
$$ LANGUAGE plpgsql;

/* D4 */
CREATE OR REPLACE FUNCTION exactMatchQueryRanking (string VARCHAR(255)) RETURNS TABLE ( postid INTEGER, rank BIGINT, body TEXT, title TEXT ) AS $$ 
DECLARE 
	q TEXT := '';
	string_elem TEXT;
	i INTEGER := 1;
BEGIN
	q := 'SELECT postid,sum(relevance) rank, body, title FROM QA_Post LEFT JOIN QA_Question ON QA_Post.postid = QA_Question.questionid ,(SELECT id, 1 relevance FROM wo WHERE id IN (';
	FOR string_elem IN SELECT regexp_split_to_table(string, ' ')
	LOOP 
		q := q || 'SELECT id FROM wi WHERE word = ''';
		q := q || string_elem;
		IF (SELECT count(*) FROM regexp_split_to_table(string, ' ')) <> i THEN
			q := q || ''' INTERSECT ';
		END IF;
		i := i+1;
	END LOOP;
	q := q || ''')) t WHERE id = postid GROUP BY postid, body, title ORDER BY rank DESC;';
	RAISE NOTICE '%', q;
	RETURN QUERY EXECUTE q;
END;
$$ LANGUAGE plpgsql;

/* D5 */
DROP TABLE IF EXISTS wi_temp;
CREATE TABLE wi_temp AS SELECT ID
,
LOWER ( word ) word, count(word) ndt, 0 nd, 0.0 nt  
FROM
	words 
WHERE
	word ~* '^[a-z][a-z0-9_]+$' 
	AND tablename = 'posts' 
	AND (what = 'body' OR what='title') 
GROUP BY
	ID,
	word;
	
DROP TABLE IF EXISTS nd;
CREATE TABLE nd AS
SELECT id, SUM(ndt) nd FROM wi_temp GROUP BY id;

UPDATE wi_temp SET nd = (SELECT nd.nd FROM nd WHERE wi_temp.id=nd.id);

DROP TABLE IF EXISTS nt;
CREATE TABLE nt AS
SELECT word, COUNT(ndt) nt FROM wi_temp GROUP BY word;

UPDATE wi_temp SET nt = (SELECT nt.nt FROM nt WHERE wi_temp.word=nt.word);

DROP TABLE IF EXISTS wo;
CREATE TABLE wo AS SELECT ID
,
word, LOG(2, 1+ndt*nd)*1/nt tfidf
FROM wi_temp;

DROP TABLE IF EXISTS wi_temp;
DROP TABLE IF EXISTS nd;
DROP TABLE IF EXISTS nt;

/* D6 */
CREATE OR REPLACE FUNCTION exactMatchQueryRankingTFIDF (string VARCHAR(255)) RETURNS TABLE ( postid INTEGER, rank NUMERIC, body TEXT, title TEXT ) AS $$ 
DECLARE 
	q TEXT := '';
	string_elem TEXT;
	i INTEGER := 1;
BEGIN
	q := 'SELECT postid,sum(tfidf) rank, body, title FROM QA_Post LEFT JOIN QA_Question ON QA_Post.postid = QA_Question.questionid ,(SELECT id, tfidf FROM wo WHERE id IN (';
	FOR string_elem IN SELECT regexp_split_to_table(string, ' ')
	LOOP 
		q := q || 'SELECT id FROM wo WHERE word = ''';
		q := q || string_elem;
		IF (SELECT count(*) FROM regexp_split_to_table(string, ' ')) <> i THEN
			q := q || ''' INTERSECT ';
		END IF;
		i := i+1;
	END LOOP;
	q := q || ''')) t WHERE id = postid GROUP BY postid, body, title ORDER BY rank DESC;';
	RAISE NOTICE '%', q;
	RETURN QUERY EXECUTE q;
END;
$$ LANGUAGE plpgsql;

/* D7 */
CREATE OR REPLACE FUNCTION wordtowords (string VARCHAR(255)) RETURNS TABLE ( word TEXT, rank BIGINT) AS $$ 
DECLARE 
	q TEXT := '';
	string_elem TEXT;
	i INTEGER := 1;
	j INTEGER := 1;
BEGIN
	q := 'SELECT word, count(*) as rank FROM wi WHERE id IN (';
	FOR string_elem IN SELECT regexp_split_to_table(string, ' ')
	LOOP 
		q := q || 'SELECT id FROM wi WHERE word = ''';
		q := q || string_elem;
		IF (SELECT count(*) FROM regexp_split_to_table(string, ' ')) <> i THEN
			q := q || ''' INTERSECT ';
		END IF;
		i := i+1;
	END LOOP;
	q := q || ''') AND (';
	FOR string_elem IN SELECT regexp_split_to_table(string, ' ')
	LOOP 
		q := q || 'word = ''';
		q := q || string_elem;
		IF (SELECT count(*) FROM regexp_split_to_table(string, ' ')) <> j THEN
			q := q || ''' OR ';
		END IF;
		j := j+1;
	END LOOP;
	q := q || ''') GROUP BY word ORDER BY rank DESC;';
	RAISE NOTICE '%', q;
	RETURN QUERY EXECUTE q;
END;
$$ LANGUAGE plpgsql;

