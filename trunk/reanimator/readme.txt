Release thread: http://www.hellgateaus.net/forum/hellgate-london-download-reanimator/reanimator-r1133/

==== Installation ====

    Simply extract zip file to desired location (also see Requirements):
        Reanimator: Run Reanimator.exe, and it should ask to set HGL path (or go to Options).
        Hellpack (command line tool): For most common file formats (.xml, .coooked, .dat), simply drag-and-drop onto hellpack.exe and it should "just work" (cook, uncook, or extract the file - if the operation is supported) - THIS IS EXPERIMENTAL and can be buggy/weird sometimes... (this feature is mostly used for debugging and quick testing, but can be useful when it works).
        For full hellpack usage, call with /help or /?
        (note: as far as I'm aware, everything and more but mod installation can be done in Reanimator)

==== Changes/Background ====

    The most significant changes relate to whatever Nagahaku needed/requested of me at the time - I'd fix, Nagahaku would test and report any bugs and etc. rinse and repeat heh.
    These were mainly related to game XML resources (uncooking and cooking) and also the level rules, as well as Hellpack integration and fixes of these... As far as I'm aware (and can remember :-[), it was all working and crash free last we spoke.
    For those interested in the details, you can check out the SVN change logs on the Google Code site (http://code.google.com/p/reanimator/), but otherwise, enjoy.


==== Requirements ====

    A Hellgate Installation
    The following are known to work:
        SP should be 100% supported.
        TC (Test Centre) should also be fully supported (though I've not tested it in a while so hopefully no bugs have been introduced since).
        Japanese was mostly supported - I'd be surprised if there weren't some table uncooking issues by now though.
        Global was mostly supported (more so) - but as above with the Japanese client however.
        <other regions should also work, but again, depends on whether there have been table structure changes>
            Since it probably needs to be said; changing the data files on your online client will likely result in a permaban for "hacking".
            The client holds a CRC value for each of the data files, and for all the individual files.
            Additionally, we in no way condone bypassing these and yada yada yada (i.e. play nice; don't be an ass).
             
    Microsoft .NET Framework 4.0 (at least)
    (sorry, but there are certain features that we do use from 4.0, so it's not that we just decided to compile to it "just because")
        For those of you that have any version installed, you should be able to just check your windows updates:
            Start > Control Panel > System and Security > Windows Update
            It will be an optional update if it's there (might be 4.5 or a lesser version - if the latter, installing the older ones should then reveal the later versions).
        For those that don't have it, or can't see the updates (or are unsure):
            Web Installer: http://www.microsoft.com/en-us/download/details.aspx?id=17851
            Standalone Installer: http://www.microsoft.com/en-us/download/details.aspx?id=24872
             
    ~10 MB HDD free (lol) - though in all seriousness, ~2 GB+ free only if you plan to extract the .dat and/or .hpd files
	

	
==== Troubleshooting ====

	The most common error is not having .Net 4.0, so do check you have this.
	Otherwise, post in the release thread.
	


Alex.
Revival Team
http://www.hellgateaus.net/