-- GROUP: raw9, MEMBERS: Baronnet, Aurelien; Føhns-Greve, Marco; Klitgaard, Christian; Nøhr, Peter
DROP SEQUENCE IF EXISTS sq_userid CASCADE;
DROP SEQUENCE IF EXISTS sq_searchhistory CASCADE;
DROP SEQUENCE IF EXISTS sq_marking CASCADE;
DROP SEQUENCE IF EXISTS sq_annotation CASCADE;
DROP TABLE IF EXISTS HM_User CASCADE;
DROP TABLE IF EXISTS HM_SearchHistory CASCADE;
DROP TABLE IF EXISTS HM_Marking CASCADE;
DROP TABLE IF EXISTS HM_Annotation CASCADE;
DROP TABLE IF EXISTS QA_Links CASCADE;
DROP TABLE IF EXISTS QA_Answer CASCADE;
DROP TABLE IF EXISTS QA_Question CASCADE;
DROP TABLE IF EXISTS QA_Describes CASCADE;
DROP TABLE IF EXISTS QA_Tag CASCADE;
DROP TABLE IF EXISTS QA_Comment CASCADE;
DROP TABLE IF EXISTS QA_Post CASCADE;
DROP TABLE IF EXISTS QA_User CASCADE;
DROP SEQUENCE IF EXISTS foo CASCADE;

CREATE TABLE QA_User
(
	userId INTEGER NOT NULL,
	displayName VARCHAR(255) NOT NULL,
	age INTEGER,
	userLocation VARCHAR(255),
	creationDate TIMESTAMP,
	
	PRIMARY KEY (userId)
);

CREATE TABLE QA_Post
(
	postId INTEGER NOT NULL,
	body TEXT NOT NULL,
	score INTEGER NOT NULL DEFAULT 0,
	creationDate TIMESTAMP NOT NULL,
	userId INTEGER NOT NULL,
	
	PRIMARY KEY (postId),
	
	FOREIGN KEY (userId)
		REFERENCES QA_User (userId)
);

CREATE TABLE QA_Comment
(
	commentId INTEGER NOT NULL,
	textContain TEXT NOT NULL,
	score INTEGER NOT NULL DEFAULT 0,
	creationDate TIMESTAMP NOT NULL,
	userId INTEGER NOT NULL,
	postId INTEGER NOT NULL,
	
	PRIMARY KEY (commentId),
	
	FOREIGN KEY (userId)
		REFERENCES QA_User (userId),
	FOREIGN KEY (postId)
		REFERENCES QA_Post (postId)
);

CREATE SEQUENCE foo START 1;
CREATE TABLE QA_Tag
(
	tagId INTEGER NOT NULL DEFAULT nextval('foo'::regclass),
	tagName VARCHAR(255) NOT NULL,
	
	PRIMARY KEY (tagId)
);

CREATE TABLE QA_Describes
(
	tagId INTEGER NOT NULL,
	postId INTEGER NOT NULL,
	
	PRIMARY KEY (tagId,postId),
	
	FOREIGN KEY (tagId)
		REFERENCES QA_Tag (tagId),
	FOREIGN KEY (postId)
		REFERENCES QA_Post (postId)
);

CREATE TABLE QA_Question
(
	questionId INTEGER NOT NULL,
	title TEXT NOT NULL,
	closedDate TIMESTAMP,
	acceptAnswer INTEGER,
	
	PRIMARY KEY (questionId),
	
	FOREIGN KEY (questionId)
		REFERENCES QA_Post (postId)
);

CREATE TABLE QA_Links
(
	questionId INTEGER NOT NULL,
	postId INTEGER NOT NULL,
	
	PRIMARY KEY (questionId,postId),
	
	FOREIGN KEY (questionId)
		REFERENCES QA_Question (questionId),
	FOREIGN KEY (postId)
		REFERENCES QA_Post (postId)
);

CREATE TABLE QA_Answer
(
	answerId INTEGER NOT NULL,
	questionId INTEGER NOT NULL,
	
	PRIMARY KEY (answerId),
	
	FOREIGN KEY (questionId)
		REFERENCES QA_Question (questionId),
	FOREIGN KEY (answerId)
		REFERENCES QA_Post (postId)
);

/* INSERT TABLES */
INSERT INTO QA_User(userId, displayName, age, userLocation, creationDate)
SELECT authorid, authordisplayname, authorage, authorlocation, authorcreationdate FROM (
	SELECT authorid, authordisplayname, authorage, authorlocation, authorcreationdate FROM comments_universal
	UNION
	SELECT ownerid as authorid, ownerdisplayname as authordisplayname, ownerage as authorage, ownerlocation as authorlocation, ownercreationdate as authorcreationdate FROM posts_universal
) as all_user;

INSERT INTO QA_Post(postId, body, score, creationDate,	userId)
SELECT DISTINCT id, body, score, creationdate, ownerid FROM posts_universal;

INSERT INTO QA_Comment(commentId,	textContain, score,	creationDate,	userId,	postId)
SELECT DISTINCT commentid, commenttext, commentscore, commentcreatedate, authorid, postid FROM comments_universal;

INSERT INTO QA_Question(questionId, title, closedDate, acceptAnswer)
SELECT DISTINCT id, title, closeddate, acceptedanswerid FROM posts_universal WHERE posttypeid = 1;

UPDATE QA_Question SET acceptanswer = NULL WHERE acceptanswer IS NOT NULL AND acceptanswer NOT IN (SELECT id FROM posts_universal);

INSERT INTO QA_Answer(answerId,	questionId)
SELECT DISTINCT id, parentid FROM posts_universal WHERE posttypeid = 2;

INSERT INTO QA_Links(questionid, postid)
SELECT id, linkpostid FROM posts_universal WHERE linkpostid IS NOT NULL AND linkpostid IN (SELECT id FROM posts_universal);

INSERT INTO QA_Tag(tagname)
SELECT DISTINCT regexp_split_to_table(tags, '::') tags FROM posts_universal WHERE tags IS NOT NULL;

INSERT INTO QA_Describes(tagid, postid)
SELECT tagid, id FROM QA_tag JOIN (SELECT DISTINCT id, regexp_split_to_table(tags, '::') as tags FROM posts_universal WHERE tags IS NOT NULL ) as test ON tagname = tags;

/* ALTER FOREIGN KEYS */
ALTER TABLE QA_Question
ADD FOREIGN KEY (acceptAnswer) REFERENCES
QA_Answer(answerId);

CREATE TABLE HM_User
(
	username VARCHAR(255) NOT NULL,
	creationDate TIMESTAMP NOT NULL,
	userPassword TEXT NOT NULL,
	email VARCHAR(255) NOT NULL,
	salt TEXT NOT NULL,
	
	PRIMARY KEY (username)
);

CREATE SEQUENCE sq_searchhistory START 1;
CREATE TABLE HM_SearchHistory
(
	searchHistoryId INTEGER NOT NULL DEFAULT nextval('sq_searchhistory'::regclass),
	searchDate TIMESTAMP NOT NULL,
	searchText VARCHAR(255) NOT NULL,
	username VARCHAR(255) NOT NULL,
	
	PRIMARY KEY (searchHistoryId),
	
	FOREIGN KEY (username)
		REFERENCES HM_User (username)
);

CREATE SEQUENCE sq_marking START 1;
CREATE TABLE HM_Marking
(
	markingId INTEGER NOT NULL DEFAULT nextval('sq_marking'::regclass),
	markingDate TIMESTAMP NOT NULL,
	username VARCHAR(255) NOT NULL,
	annotation VARCHAR(255),
	postid INTEGER,
	commentid INTEGER,
	
	PRIMARY KEY (markingId),
	
	FOREIGN KEY (username)
		REFERENCES HM_User (username),
	FOREIGN KEY (postid)
		REFERENCES QA_Post (postid),
	FOREIGN KEY (commentid)
		REFERENCES QA_Comment (commentid)
);

/*DROP TABLE IF EXISTS comments_universal CASCADE;
DROP TABLE IF EXISTS post_universal CASCADE;*/