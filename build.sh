#!/bin/sh
# Builds 1.5/1.6 from Source/; RimWorld 1.7 from Source17/ (Waylith port).
set -e
ROOT="$(dirname "$0")"
rm -rf "$ROOT/Assemblies"
cd "$ROOT/Source"
rm -rf obj
for v in 1.6 1.5; do
  echo "===== Building RimWorld $v from Source ====="
  dotnet build -c Release -nologo -p:RimWorldVersion=$v
done
cd "$ROOT/Source17"
rm -rf obj
echo "===== Building RimWorld 1.7 from Source17 ====="
dotnet build -c Release -nologo
echo "Done. 1.5/Assemblies, 1.6/Assemblies, and 1.7/Assemblies are ready."
