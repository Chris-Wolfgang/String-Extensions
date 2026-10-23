type: internal

`PadCenter` allocates only its result string on .NET 8 and later, and no longer allocates two temporary padding strings on the older targets; the result is unchanged.
