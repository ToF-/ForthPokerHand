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
