# How to push PackageManager

- official document
  - https://developer.rhino3d.com/guides/yak/

## Summary

1. Build component
1. Update version info in manifest.yml
    - e.g. "1.1.0-beta.1" to treat it as a pre-release.
2. Move some items to package folder like following.
    - <img width="200" src="./folder_structure.jpg">
3. Use `"C:\Program Files\Rhino 7\System\Yak.exe" build` command to build yak package.
    - If the build runs without problems, the guid is added to the keywords in manifest.
4. Rename the package to tunny-x.y.z-rh7_13-win.yak
   - Since tunny is not running on rhino6 and mac.
5. Use `"C:\Program Files\Rhino 7\System\Yak.exe" push tunny-x.y.z-rh7-win.yak` command to push the package to Rhino PackageManager.
6. Finish!