-- GROUP: raw9, MEMBERS: Baronnet, Aurelien; Føhns-Greve, Marco; Klitgaard, Christian; Nøhr, Peter
/* D1 */ 
SELECT * FROM hm_seachhistory;

INSERT INTO hm_user(displayname, creationdate, userpassword, email)
VALUES ('test123', NOW(),'test123','test123@gmail.com');

SELECT * FROM searchString('java','1');

SELECT * FROM hm_seachhistory;

/* D2 */ 
SELECT * FROM wi;

/* D3 */ 
SELECT * FROM exactMatchQuery('java help me');

/* D4 */ 
SELECT * FROM exactMatchQueryRanking	('java help me');

/* D6 */
SELECT * FROM exactMatchQueryRankingTFIDF	('java help me');

/* D7 - D8 */ 
SELECT * FROM wordtowords('java help me');
