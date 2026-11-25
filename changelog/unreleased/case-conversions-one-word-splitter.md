type: breaking

`ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` now split a string into words by one shared rule: a word is a run of letters and digits, and every other character (white space, punctuation, symbols, control characters, `_` and `-`) is a word boundary that no longer appears in the result. `"hello, world!".ToSnakeCase()` returns `hello_world`, not `hello,_world!`; `"user_login_count".ToPascalCase()` returns `UserLoginCount`; `"(hello) world".ToPascalCase()` returns `HelloWorld`. An apostrophe is removed without ending the word.
