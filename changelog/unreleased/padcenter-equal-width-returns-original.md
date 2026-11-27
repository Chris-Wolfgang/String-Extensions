type: fix

`PadCenter` now returns the original string instance, without allocating a copy, when `totalWidth` equals the string's length, as the README already said.
