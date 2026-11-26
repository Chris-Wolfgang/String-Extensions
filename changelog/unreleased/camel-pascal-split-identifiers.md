type: fix

`ToCamelCase` and `ToPascalCase` now keep the words of a PascalCase or camelCase identifier apart: `"userLoginCount".ToPascalCase()` returns `UserLoginCount`, not `Userlogincount`, and `"UserLoginCount".ToCamelCase()` returns `userLoginCount`. An acronym becomes one word (`SKUCode` is `SkuCode`).
