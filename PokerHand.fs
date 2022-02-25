
14 CONSTANT ACE
0 CONSTANT HEARTS

: STRING>CARD ( addr,u -- card )
    2DROP   \ fake, do nothing with the address and length
    0       \ fake
;

: RANK ( card -- rank )
    DROP ACE ;  \ fake it

: SUIT ( card -- suit )
    DROP HEARTS ;  \ fake it
