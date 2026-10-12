type: internal

Turn off implicit and global usings in the multi-target library and test projects; every file now states its own `using` directives, so each one is needed on every target framework.
