REQUIRE ffl/tst.fs
REQUIRE PokerHand.fs

: TEST-STRING-CARD-RANK-SUIT
    ." a card value can be created from a string." CR
    ." one can extract the rank and suit from a card value." CR
    S" AH" STRING>CARD DUP RANK ACE    ?S SUIT HEARTS ?S
    S" KH" STRING>CARD DUP RANK KING   ?S SUIT HEARTS ?S
    S" QS" STRING>CARD DUP RANK QUEEN  ?S SUIT SPADES ?S
    S" JS" STRING>CARD DUP RANK JACK   ?S SUIT SPADES ?S
    S" TD" STRING>CARD DUP RANK TEN    ?S SUIT DIAMONDS ?S
    S" 9C" STRING>CARD DUP RANK 9      ?S SUIT CLUBS   ?S
    S" 2H" STRING>CARD DUP RANK 2      ?S SUIT HEARTS   ?S
;

: TEST-CARD-CREATION
    ." cards are created as constant words whose name represent the card value." CR
    AH DUP RANK ACE ?S SUIT HEARTS ?S
    1S DUP RANK 1   ?S SUIT SPADES ?S
    KC DUP RANK KING ?S SUIT CLUBS ?S
    QD DUP RANK QUEEN ?S SUIT DIAMONDS ?S
    JS RANK JACK ?S
    AH KH QH JH TH 9H 8H 7H 6H 5H 4H 3H 2H 1H 15 1 DO DUP SUIT HEARTS ?S RANK I ?S LOOP
    AS KS QS JS TS 9S 8S 7S 6S 5S 4S 3S 2S 1S 15 1 DO DUP SUIT SPADES ?S RANK I ?S LOOP
    AD KD QD JD TD 9D 8D 7D 6D 5D 4D 3D 2D 1D 15 1 DO DUP SUIT DIAMONDS ?S RANK I ?S LOOP
    AC KC QC JC TC 9C 8C 7C 6C 5C 4C 3C 2C 1C 15 1 DO DUP SUIT CLUBS ?S RANK I ?S LOOP
;

2 BASE !
: TEST-HAND-CREATION
    0 JH 8D 4S AD 7S 5C KH CARDS>HAND DUP
\ --K   H --5   C --7   S --A   D --4   S --8   D --J   H
  00110100000101110001110100111010000100010010001000101100 ?S
  HAND>CARDS KH ?S 5C ?S 7S ?S AD ?S 4S ?S 8D ?S JH ?S 0 ?S
    0 JH 8D CARDS>HAND DUP
\ --8   D --J   H
  0010001000101100 ?S
  HAND>CARDS 8D ?S JH ?S 0 ?S
;
DECIMAL

VARIABLE MYCOUNTER
: TEST-COUNTER
    ." a counter can count occurrences of numbers from 0 to 14 when they occur between 0 and 7 times" CR
    MYCOUNTER OFF
    3 MYCOUNTER INCREASE
    3 MYCOUNTER INCREASE
    14 MYCOUNTER INCREASE
    14 MYCOUNTER INCREASE
    14 MYCOUNTER INCREASE
    3 MYCOUNTER COUNT@ 2 ?S
    5 MYCOUNTER COUNT@ 0 ?S
    14 MYCOUNTER INCREASE
    14 MYCOUNTER COUNT@ 4 ?S
;


TEST-STRING-CARD-RANK-SUIT
TEST-CARD-CREATION
TEST-HAND-CREATION
TEST-COUNTER
BYE
