INCLUDE ffl/tst.fs

0 CONSTANT HEARTS
1 CONSTANT SPADES
2 CONSTANT DIAMONDS
3 CONSTANT CLUBS

10 CONSTANT TEN
11 CONSTANT JACK
12 CONSTANT QUEEN
13 CONSTANT KING
14 CONSTANT ACE

: CHAR>RANK ( c -- r )
         DUP [CHAR] T = IF DROP TEN
    ELSE DUP [CHAR] J = IF DROP JACK
    ELSE DUP [CHAR] Q = IF DROP QUEEN
    ELSE DUP [CHAR] K = IF DROP KING
    ELSE DUP [CHAR] A = IF DROP ACE
    ELSE [CHAR] 0 -
    THEN THEN THEN THEN THEN ;

: CHAR>SUIT ( c -- s )
         DUP [CHAR] H = IF DROP HEARTS
    ELSE DUP [CHAR] S = IF DROP SPADES
    ELSE DUP [CHAR] D = IF DROP DIAMONDS
    ELSE DROP CLUBS THEN THEN THEN ;

: RANKSUIT>CARD ( r,s -- card )
    SWAP 2 LSHIFT OR ;

: RANK ( card -- rank )
    2 RSHIFT 15 AND ;

: SUIT ( card -- suit )
    3 AND ;

: STRING>CARD ( addr,u -- card )
    DROP
    DUP     C@ CHAR>RANK
    SWAP 1+ C@ CHAR>SUIT
    RANKSUIT>CARD ;

: CARD ( "card" -- creates a constant named <card> )
    CREATE LATEST NAME>STRING STRING>CARD , DOES> @ ;

CARD 1H CARD 2H CARD 3H CARD 4H CARD 5H CARD 6H CARD 6H CARD 7H CARD 8H CARD 9H CARD TH CARD JH CARD QH CARD KH CARD AH
CARD 1S CARD 2S CARD 3S CARD 4S CARD 5S CARD 6S CARD 6S CARD 7S CARD 8S CARD 9S CARD TS CARD JS CARD QS CARD KS CARD AS
CARD 1D CARD 2D CARD 3D CARD 4D CARD 5D CARD 6D CARD 6D CARD 7D CARD 8D CARD 9D CARD TD CARD JD CARD QD CARD KD CARD AD
CARD 1C CARD 2C CARD 3C CARD 4C CARD 5C CARD 6C CARD 6C CARD 7C CARD 8C CARD 9C CARD TC CARD JC CARD QC CARD KC CARD AC


: GROUPSIZE ( card -- n )
    6 RSHIFT 3 AND 1+ ;

CREATE RANK-COUNTS 15 ALLOT

: CARDS>HAND ( a,b,c,d,e,n -- h )
    1- 0 DO 8 LSHIFT OR LOOP ;

: HAND>CARDS ( h,n -- a,b,c,d,e )
    1- 0 DO DUP 255 AND SWAP 8 RSHIFT LOOP ;

: 5DUP ( a,b,c,d,e -- a,b,c,d,e,a,b,c,d,e )
    5 CARDS>HAND DUP >R
    5 HAND>CARDS R> 5 HAND>CARDS ;

: INCREASE-RANK-COUNT ( card -- )
    RANK RANK-COUNTS + DUP C@ 1+ SWAP C! ;

: COUNT-CARDS ( c1,c2,c3,c4,c5 -- c1,c2,c3,c4,c5 with group size value )
    RANK-COUNTS 15 ERASE
    5DUP
    5 0 DO INCREASE-RANK-COUNT LOOP
    5 0 DO 4 ROLL DUP RANK RANK-COUNTS + C@ 1- 6 LSHIFT OR LOOP ;

2 BASE ! 11111100 CONSTANT CARDCOMPAREMASK DECIMAL

: CARDCOMPARE ( card,card -- -x|0|+x )
    SWAP CARDCOMPAREMASK AND
    SWAP CARDCOMPAREMASK AND
    - ;

: 2CARDSORT ( card,card -- card,card )
    2DUP CARDCOMPARE 0 < IF SWAP THEN ;

: 3CARDSORT ( card,card,card -- card,card,card )
    2CARDSORT >R 2CARDSORT R> 2CARDSORT ;

: 4CARDSORT ( card,card,card,card -- card,card,card,card )
    3CARDSORT >R 3CARDSORT R> 3CARDSORT ;

: 5CARDSORT ( a,d,b,c,e -- e,d,c,b,a )
    4CARDSORT >R 4CARDSORT R> 4CARDSORT ;

: SORT-CARDS ( 5 cards -- 5 sorted cards )
    COUNT-CARDS
    5CARDSORT ;

: INIT-DISCARDED ( -- i,j )
    0 0 ;

: NEXT-DISCARDED ( i,j -- i,j+1|i+1,i+1+1 )
    1+ DUP 7 = IF DROP 1+ DUP 1+ 7 = IF DROP 0 0 ELSE DUP 1+ THEN THEN ;

: SUBSEQUENCE ( i,j -- [0,1,2,3,4,5,6] minus { i,j } )
    >R >R 7 0 DO I LOOP
    6 R> - ROLL DROP
    5 R> 1 - - ROLL DROP ;

0 CONSTANT  HIGHCARD
1 CONSTANT  ONEPAIR
2 CONSTANT  TWOPAIR
3 CONSTANT  THREEOFAKIND
4 CONSTANT  STRAIGHT
8 CONSTANT  FLUSH
9 CONSTANT  FULLHOUSE
10 CONSTANT  FOUROFAKIND
12 CONSTANT  STRAIGHTFLUSH

2 base !
CREATE groupcategories
0000000000 , highcard ,
0101000000 , onepair ,
0101010100 , twopair ,
1010100000 , threeofakind ,
1010100101 , fullhouse ,
1111111100 , fourofakind ,
decimal

: 5reverse ( a,b,c,d,e -- e,d,c,b,a )
    -rot swap         \ a,b,e,d,c
    >r 2swap swap     \ e,d,b,a
    r> -rot ;         \ e,d,c,b,a 

: group-pattern ( a,b,c,d,e -- gp )
    5reverse 
    0 5 0 DO 2 LSHIFT SWAP GROUPSIZE 1- OR LOOP ;

: find-group-category ( a,b,c,d,e -- gc )
    group-pattern groupcategories 
    begin 
        over over @ <> while
        cell+ cell+
    repeat
    nip cell+ @ ;

: flush? ( a,b,c,d,e -- f )
    >R >R >R >R 
    suit r> suit r> suit r> suit r> suit 
    -1 4 0 do if over = else drop 0 then loop
    nip ;
    
: straight? ( a,b,d,d,e -- f )
    >R >R >R >R 
    rank R> rank r> rank r> rank r> rank
    over - >r over - >r over - >r over - >r drop
    r> r> + r> + r> + -4 = ;

: TEST-CHAR>RANK
    ." CHAR>RANK converts char 123456789TJQKA to rank value" CR
    [CHAR] 1 CHAR>RANK 1 ?S
    [CHAR] 7 CHAR>RANK 7 ?S
    [CHAR] T CHAR>RANK TEN ?S
    [CHAR] J CHAR>RANK JACK ?S
    [CHAR] Q CHAR>RANK QUEEN ?S
    [CHAR] K CHAR>RANK KING ?S
    [CHAR] A CHAR>RANK ACE ?S
;

: TEST-CHAR>SUIT
    ." CHAR>SUIT converts char HSDC to suit value" CR
    [CHAR] H CHAR>SUIT HEARTS ?S
    [CHAR] S CHAR>SUIT SPADES ?S
    [CHAR] D CHAR>SUIT DIAMONDS ?S
    [CHAR] C CHAR>SUIT CLUBS ?S
;

: TEST-RANKSUIT>CARD
    ." RANKSUIT>CARD converts a rank and a suit int o a card" CR
    13 3 RANKSUIT>CARD
    DUP RANK KING ?S
        SUIT CLUBS ?S
;

: TEST-STRING>CARD
    ." STRING>CARD converts a string into a card" CR
    S" KC" STRING>CARD
    DUP RANK KING ?S
        SUIT  CLUBS ?S
;

: TEST-CARDS
    ." CARD can create a constant card" CR
    1H S" 1H" STRING>CARD ?S
    AC S" AC" STRING>CARD ?S
    QD S" QD" STRING>CARD ?S
    KH RANK KING ?S
    KH SUIT HEARTS ?S
;

: TEST-HAND-CARDS
    ." CARDS>HAND converts cards to a single cell hand" CR
    KH 2C KD 3S 3D 5 CARDS>HAND
    ." HAND>CARDS converts a single cell hand into 5 cards" CR
    5 HAND>CARDS
    3D ?S 3S ?S KD ?S 2C ?S KH ?S
;

: TEST-GROUPSIZE
    ." before counting the GROUPSIZE of a card is 1" CR
    KH GROUPSIZE 1 ?S
    ." after counting the GROUPSIZE of a card is between 1 and 4 depending on cards" CR
    KH 2C KD 3S 3D COUNT-CARDS
    DUP DUP RANK 3 ?S SUIT DIAMONDS ?S GROUPSIZE 2 ?S
    DUP DUP RANK 3 ?S SUIT SPADES ?S GROUPSIZE 2 ?S
    DUP DUP RANK KING ?S SUIT DIAMONDS ?S GROUPSIZE 2 ?S
    DUP DUP RANK 2 ?S SUIT CLUBS ?S GROUPSIZE 1 ?S
    DUP DUP RANK KING ?S SUIT HEARTS ?S GROUPSIZE 2 ?S
    KH 2C KD KC KS COUNT-CARDS
    DUP DUP RANK KING ?S SUIT SPADES ?S GROUPSIZE 4 ?S
    2DROP 2DROP
;

: TEST-SORTCARDS
    ." SORTCARDS sorts and counts a group of 5 cards by groupsize and rank descending" CR
    4H KS JS 8H 8C SORT-CARDS
    DUP DUP RANK 4 ?S SUIT HEARTS ?S GROUPSIZE 1 ?S
    DUP DUP RANK JACK ?S SUIT SPADES ?S GROUPSIZE 1 ?S
    DUP DUP RANK KING ?S SUIT SPADES ?S GROUPSIZE 1 ?S
    DUP DUP RANK 8 ?S SUIT CLUBS ?S GROUPSIZE 2 ?S
    DUP DUP RANK 8 ?S SUIT HEARTS ?S GROUPSIZE 2 ?S ;

: TEST-DISCARDED
    ." DISCARDED cards index from 0 to 7 are initially 0 and 1" CR
    INIT-DISCARDED
    NEXT-DISCARDED 2DUP SWAP 0 ?S 1 ?S
    ." NEXT-DISCARDED increases discarded cards index following 0,2 0,3 .. 1,2 1,3 .. until 5,6" CR
    NEXT-DISCARDED 2DUP SWAP 0 ?S 2 ?S
    NEXT-DISCARDED 2DUP SWAP 0 ?S 3 ?S
    NEXT-DISCARDED 2DUP SWAP 0 ?S 4 ?S
    NEXT-DISCARDED 2DUP SWAP 0 ?S 5 ?S
    NEXT-DISCARDED 2DUP SWAP 0 ?S 6 ?S
    NEXT-DISCARDED 2DUP SWAP 1 ?S 2 ?S
    ." NEXT-DISCARDED increases discarded cards index until 5,6 then makes it 0,0 for flag" CR
    13 0 DO NEXT-DISCARDED LOOP
    NEXT-DISCARDED 2DUP SWAP 5 ?S 6 ?S
    NEXT-DISCARDED 0 ?S 0 ?S
;

: TEST-SUBSEQUENCE
    ." SUBSEQUENCE count numbers from 0 to 6 except the two discarded ones" CR
    0 6 SUBSEQUENCE 5 ?S 4 ?S 3 ?S 2 ?S 1 ?S
    1 2 SUBSEQUENCE 6 ?S 5 ?S 4 ?S 3 ?S 0 ?S
    5 6 SUBSEQUENCE 4 ?S 3 ?S 2 ?S 1 ?S 0 ?S
;

: test-find-group-category
    ." find-group-category find the group category of a sorted hand" cr
    3H 5H QH AS 7S sort-cards find-group-category highcard ?S 
    3H 3D AH QS 7H sort-cards find-group-category onepair ?S
    3H 3D AH QS AH sort-cards find-group-category twopair ?S
    3H 6D AH AS AH sort-cards find-group-category threeofakind ?S
    3H 3D AH AS AH sort-cards find-group-category fullhouse ?S
    3H AD AH AS AH sort-cards find-group-category fourofakind ?S
;

: test-flush?
    ." flush? find if all cards have same suit" cr
    3H 5H QH AS 7S flush? 0 ?S
    3H 5H QH AH 7H flush? -1 ?S
    3S 5H QH AH 7H flush? 0 ?S
    3H 5S QH AH 7H flush? 0 ?S
; 

: test-straight?
    ." straight? find if all cards are in sequence" cr
    3H 5H QH AS 7S sort-cards straight? 0 ?S
    3H 5H 4H 6S 7S sort-cards straight? -1 ?S
;



: TESTS
    TEST-CHAR>RANK
    TEST-CHAR>SUIT
    TEST-RANKSUIT>CARD
    TEST-STRING>CARD
    TEST-CARDS
    TEST-HAND-CARDS
    TEST-GROUPSIZE
    TEST-SORTCARDS
    TEST-DISCARDED
    TEST-SUBSEQUENCE
    test-find-group-category
    test-flush?
    test-straight?
;

TESTS
BYE
