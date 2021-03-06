# FileDownloader     

#Latest Changes
1. Restructured whole project - into separate namespace and folder
2. DTO - FileConnInfo, Service - Download Client with Factory, Processor - Download Strategy with Factory
3. Created new configuration object - to have all required config in one place
4. Test coverage is incomplete, added few more unit test files
5. Network Simulator still needs to added
6. Manually tested download strategy - all sequential, parallel and parallel with retry
7. MyFileDownloader - Big composite file download processor, based on config driven strategy.

##TODOs
1. Full Test Coverage
2. More Extensible Network Simulator
3. Statistics/Speed driven download strategy scheduler


## Features
1. Exception Handling    
2. Logging
3. Configurable source and download location
4. Supports SFTP or FTP     
   a) FTPCient - Wrapper over FtpWebRequest & FtpWebResponse    
   b) SFTPClient - Wrapper over Sharp.SSH for SFTP     
5. Streaming strategy to read big files over network      
6. Partially downloaded files are deleted in case of failure of source or other exception while reading
7. Extensible to new client, implementing IDownloader interface
8. Extensible Download Strategy - currently support sequential, parallel, parallel with retry

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
   **TODO: If close our application while running, its not deleting file - added a placeholder - will add this support** 
   
## Steps
1. *Read Download Location*   
2. *Read Sources*      
3. *Parse source list into array of FileConnInfo object*     
4. *Process request for each source*    


##Tips
1. Used Core FTP Server - to setup FTP and SFTP server 
