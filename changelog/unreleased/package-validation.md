type: internal

Every pack validates the package against the last published version (0.1.1) with PackageValidation, so an unintended binary-breaking change fails the build instead of reaching NuGet.
