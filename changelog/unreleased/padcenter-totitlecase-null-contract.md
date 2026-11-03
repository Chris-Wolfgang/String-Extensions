type: fix

`ToTitleCase` now throws `ArgumentNullException` with `ParamName` `s` (it used to come from `TextInfo` as `str`), and `PadCenter` declares its receiver as non-nullable `string`, so passing a maybe-null string gives a compiler warning instead of only a runtime exception.
