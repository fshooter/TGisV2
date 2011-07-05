cd /d %~dp0
rename "GisDb.db.broken" "GisDb.db.autorepair"
echo .dump | sqlite3 GisDb.db.autorepair | sqlite3 GisDb.db