REM We expect 5 Arguments
REM 1 : Configuration Name
REM 2 : TargetDir
REM 3 : TargetName
REM 4 : TargetExt
REM 5 : NuGet TargetLib
REM
IF %5 == "" (
  ECHO At least 5 Arguments are expected
  EXIT /B 1
)
IF NOT %1 == "Release" (
   ECHO Only Release version is used for NuGet, copying nothing.
   EXIT /B 0
)
IF NOT EXIST NuGetBuild MKDIR NuGetBuild
IF NOT EXIST NuGetBuild\%1 MKDIR NuGetBuild\%1
IF NOT EXIST NuGetBuild\%1\lib MKDIR NuGetBuild\%1\lib
IF NOT EXIST NuGetBuild\%1\lib\%5 MKDIR NuGetBuild\%1\lib\%5

IF NOT EXIST %2%3%4 (
  ECHO %2%3%4 not found
  EXIT /B 1
)
COPY /B /Y %2%3%4 NuGetBuild\%1\lib\%5\
IF EXIST %2%3%4.config COPY /Y /B %2%3%4.config NuGetBuild\%1\lib\%5
IF EXIST %2%3.pdb      COPY /Y /B %2%3.pdb NuGetBuild\%1\lib\%5
IF EXIST %2%3.xml      COPY /Y /B %2%3.xml NuGetBuild\%1\lib\%5

FOR %%%l IN (bg bg-bg cs cs-cz da da-dk de de-de el el-gr en en-gb en-us es es-es et et-ee fi fi-fi fr fr-fr hr hr-hr hu hu-hu it it-it ja ja-jp ko ko-kr lt lt-lt lv lv-lv nb nb-no nl nl-nl pl pl-pl pt pt-br pt-pt ro ro-ro ru ru-ru sk sk-sk sl sl-si sr sr-latn-rs sv sv-se tr tr-tr uk uk-ua zh zh-cn zh-hk zh-tw) DO (
  IF EXIST %2%%l (
    IF NOT EXIST NuGetBuild\%1\lib\%5\%%l MKDIR NuGetBuild\%1\lib\%5\%%l
    COPY /Y /B %2%%l\*.resources.dll NuGetBuild\%1\lib\%5\%%l\
  )
)

EXIT /B 0
