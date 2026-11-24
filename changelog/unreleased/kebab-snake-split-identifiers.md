type: fix

`ToSnakeCase` and `ToKebabCase` now split a PascalCase or camelCase identifier into its words: `"UserLoginCount".ToSnakeCase()` returns `user_login_count`, not `userlogincount`. Acronyms stay together (`SKUCode` is `sku_code`) and a digit stays with the word before it (`Line2Qty` is `line2_qty`). Text that already uses separators converts as before.
