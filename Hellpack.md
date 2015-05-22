# Introduction #

When your creating modifications, you want a quick way to compile your projects without having to drudge around with a gui to perform minimalistic and frequent operations.

Hellpack is a utility thats default behaviour is to scan its current directory for files that can be compiled and packed into a distributable .idx .dat combination. There are a number of switches available that run modify the program behaviour that can be easily put into a .bat file for quick double click access. These topics are described in more detail below.


# Details #

The default behaviour of Hellpack is to compile all raw (.txt) files into serialized formats (.cooked) and then packed into Hellgate format data packages (.idx, .dat). From the directory Hellpack resides it searches for 2 primary directories: **data** and **data\_common**. These directories are deep scanned and all files are packed except for the raw versions [default](by.md). This behaviour can be modified using command line arguments.

| switch | description | default |
|:-------|:------------|:--------|
| /t     | Cook excel tables. | On      |
| /x     | Cook xml files. | On      |
| /lr    | Cook level rules. | On      |
| /rd    | Cook room definitions. | On      |
| /p     | Pack all files into .idx .dat | On      |
| /s     | Search current directory for files. | On      |
| /e     | Do not pack source files. | On      |
| /p:name | name = .idx .dat filename | sp\_hellgate\_1337 |
| /h:path | path = path to hellgate installation |         |

# Usage Example #

This article assumes you have a little knowledge with the modding terminology.

So you have Hellgate London installed here: C:\Hellgate London

You may like to put Hellpack inside this directory (along with its required .dlls). This is the most convenient place to execute it when designing mods.

Now you probably want to add some excel tables to your project as they are the heart of Hellgate mods. This is quickly achieved utilizing the File Explorer from inside Reanimator. From the File Explorer, click the top tab 'Advanced'. Next to the button that says Excel files, set this path to your Hellgate London directory then click the button. This gives you a source copy of every excel table that can be edited in Open Office/Excel/Notepad and/or imported and exported from Reanimator. You'll find there are things you can do in Excel that you can't with Reanimator and vice versa.

Using Windows file explorer, browse your Hellgate London directory. Then browse the **data/excel** and **data\_common/excel** paths and there should be ~150 .txt files. For more information on importing/exporting these files into various programs please see another wiki page.

After you've edited them, its time to test your modification! To compile it, simply double click Hellpack and presuming nothing goes wrong, the .idx and .dat will be placed in the **data** directory and ready for immediate usage - just lauch Hellgate. Repeat this process whenever you update any files and want Hellgate to use them. As a modder, you will be doing this a lot!