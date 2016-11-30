# FileDownloader     
## How it works
1. **Specify Download Location**     
   App.Config - Key - "FileDownloadPath"      
2. **Specify Sources** - (_Supported ftp or sftp_)    
   a)  either via command line args   
   b)  or, from configured filename in Key - "SourcesPath"    
       File should conatin one source per line           

## Design Considerations
1. **The program should extensible to support different protocols**    
   We can add new class **ProtocolClient** which should implements **IDownloader** like FTPClient or SFTP Client.    
   Additionaly, We should handle case for new client based on schema in **DownloadFactoryClient**
2. **some sources might very big (more than memory)**     
   Its responsiblity of client class to handle reading in chunks via stream rather than reading whole file at once.    
   See FTPClient class for example.
3. **TODO: some sources might be very slow, while others might be fast**    
4. **some sources might fail in the middle of download**    
   If source fail in middle, it gracefully handle this exception, moving to next source in list
5. **we don't want to have partial data in the final location in any case.**     
   Partially downloaded file are deleted- in case of exception while receving response or source fail in middle     
   **TODO: IF application fails - added a placeholder - will add this support** 
   
##Steps
1. *Read Download Location*   
2. *Read Sources*      
3. *Parse source list into array of FileConnInfo object*     
4. *Process request for each source*    
