module Rules

let participles = 
    """
    There are 3 cases worth remembering.

    1) Pattern "minout" - those verbs have ending "nout" prepended with "r", "l" or a vowel.
    For those - remove "nout" and add "nul":
    - minout -> minul
    - shrnout -> shrnul
    - přilnout -> přilnul

    2) Pattern "tisknout" - all other verbs ending with "nout".
    For those - remove "nout" and add "l":
    - tisknout -> tiskl
    - spolehnout -> spolehl
    - hrknout -> hrkl

    3) Common pattern - all other verbs whatsoever.
    For those - remove "t" and add "l":
    - dělat -> dělal
    - hodit -> hodil
    - vidět -> viděl

    Everything else is considered here as exceptions.
    """

let comparatives =
    """
    To form comparative of the adjective, do the following.

    1) Remove the ending ("í" or "ý")

    2) Alternate stem ending:
    - ch -> š
    - sk -> št
    - ck -> čt
    - čk -> čt
    - h -> ž
    - k -> č
    - r -> ř

    3) Add comparative suffix depending on the new stem ending:
    - d, t, n, v, m, b, p -> add "ější"
    - ř, č, ž, š, l, z, s -> add "ejší"
    
    Examples:
    - pomalý -> pomalejší
    - chytrý -> chytřejší
    - ubohý -> ubožejší
    """
