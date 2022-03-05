10 CONSTANT TEN
11 CONSTANT JACK
12 CONSTANT QUEEN
13 CONSTANT KING
14 CONSTANT ACE
0 CONSTANT HEARTS
1 CONSTANT SPADES
2 CONSTANT DIAMONDS
3 CONSTANT CLUBS

\ converts a char {0123456789TJQKA} to a rank value
: CHAR>RANK ( c -- rank )
    DUP [CHAR] T = IF DROP TEN   ELSE
    DUP [CHAR] J = IF DROP JACK  ELSE
    DUP [CHAR] Q = IF DROP QUEEN ELSE
    DUP [CHAR] K = IF DROP KING  ELSE
    DUP [CHAR] A = IF DROP ACE   ELSE
    DUP [CHAR] 0 >= OVER [CHAR] 9 <= AND IF
        [CHAR] 0 -
    ELSE
        DROP S" unknown rank letter" EXCEPTION THROW
    THEN THEN THEN THEN THEN THEN ;

\ converts a char {HSDC} to a suit value
: CHAR>SUIT ( c -- suit )
    DUP [CHAR] H = IF DROP HEARTS ELSE
    DUP [CHAR] S = IF DROP SPADES ELSE
    DUP [CHAR] D = IF DROP DIAMONDS ELSE
    DUP [CHAR] C = IF DROP CLUBS ELSE
    DROP S" unknown suit letter" EXCEPTION THROW
    THEN THEN THEN THEN ;

\ converts a string into a rank and value, then make it a single byte card value
: STRING>CARD ( addr,u -- card )
    2 <> IF S" incorrect string length for a card" EXCEPTION THROW
    ELSE
        DUP C@  CHAR>RANK    2 LSHIFT
        SWAP 1+ C@ CHAR>SUIT OR
    THEN ;

: RANK ( card -- rank )
    2 RSHIFT ;

: SUIT ( card -- suit )
    3 AND ;

\ creates a new card (constant word)
: CARD ( [name] -- )
    CREATE
    LATEST NAME>STRING STRING>CARD ,
    DOES> @ ;

\ creates a hand from a number of cards
: CARDS>HAND ( 0,c1..cn -- hand )
    0 BEGIN
        SWAP ?DUP WHILE
        SWAP 8 LSHIFT SWAP OR
    REPEAT ;

\ deconstruct a hand into cards, skipping cancelled cards (=255)
: HAND>CARDS ( hand -- 0,c1..cn )
    0 SWAP BEGIN
        ?DUP WHILE
        DUP 255 AND
        DUP 255 <> IF SWAP ELSE DROP THEN
        8 RSHIFT
    REPEAT ;

CARD 1H CARD 2H CARD 3H CARD 4H CARD 5H CARD 6H CARD 7H CARD 8H CARD 9H CARD TH CARD JH CARD QH CARD KH CARD AH
CARD 1S CARD 2S CARD 3S CARD 4S CARD 5S CARD 6S CARD 7S CARD 8S CARD 9S CARD TS CARD JS CARD QS CARD KS CARD AS
CARD 1D CARD 2D CARD 3D CARD 4D CARD 5D CARD 6D CARD 7D CARD 8D CARD 9D CARD TD CARD JD CARD QD CARD KD CARD AD
CARD 1C CARD 2C CARD 3C CARD 4C CARD 5C CARD 6C CARD 7C CARD 8C CARD 9C CARD TC CARD JC CARD QC CARD KC CARD AC

: NTH-NIBBLE@ ( values,n -- value )
    4 * RSHIFT 15 AND ;

: NTH-NIBBLE! ( values,value,n -- values' )
    15 AND ROT OVER           \ value,n,values,n
    15 SWAP 4 * LSHIFT -1 XOR \ value,n,values,andmask
    AND -ROT 4 * LSHIFT OR ;

: INCREASE ( n,addr -- )
    DUP @ ROT                \ addr,values,n
    OVER OVER NTH-NIBBLE@    \ addr,values,n,value
    1+ SWAP NTH-NIBBLE!      \ addr,values'
    SWAP ! ;

: COUNT@ ( n,addr -- value )
    @ SWAP NTH-NIBBLE@ ;

: CANCEL-CARD ( hand,n -- hand' )
    255 SWAP 8 * LSHIFT OR ;
