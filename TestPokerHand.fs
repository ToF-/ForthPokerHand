REQUIRE ffl/tst.fs
REQUIRE PokerHand.fs

: TEST-STRING-CARD-RANK-SUIT
    ." a card value can be created from a string." CR
    S" AH" STRING>CARD
    ." one can extract the rank and suit from a card value." CR
    DUP RANK ACE    ?S
        SUIT HEARTS ?S
;

TEST-STRING-CARD-RANK-SUIT
BYE
