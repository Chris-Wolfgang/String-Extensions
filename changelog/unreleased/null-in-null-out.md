type: breaking

Every method now accepts a `null` string instead of throwing `ArgumentNullException`: the case conversions return `null`, `PadCenter` returns `totalWidth` padding characters and `SplitWords` returns an empty sequence, while a non-null input still gives a non-null result (`[return: NotNullIfNotNull]`) and a `null` `CaseConversionOptions` or a negative length still throws.
