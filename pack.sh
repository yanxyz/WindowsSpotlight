#!/bin/sh

name="WindowsSpotlight"
d="$name/bin/Release"
zip -j "$name.zip" \
	$d/$name.exe \
    $d/fastjson.dll

f="readme.txt"
cp README.md $f
echo "[Source](https://github.com/yanxyz/$name)" >> $f
zip -ml "$name.zip" $f
