F:
CD\
REM CD\IB\EMS 2016
rem PAUSE
for /d /r . %%d in (bin,obj,de,es,ja,ru,Express,Debug,Release) do @if exist "%%d" rd /s/q "%%d"
del *.log /s