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

TEST-STRING-CARD-RANK-SUIT
BYE
