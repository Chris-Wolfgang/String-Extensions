type: fix

`ToSnakeCase` and `ToKebabCase` now split a PascalCase or camelCase identifier into its words and put exactly one delimiter between words: `"UserLoginCount".ToSnakeCase()` returns `user_login_count`, not `userlogincount`, and `"  two  words  "` returns `two_words`, not `__two__words__`. Acronyms stay together (`SKUCode` is `sku_code`) and a digit stays with the word before it (`Line2Qty` is `line2_qty`).
