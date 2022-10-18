using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Windows;
using ICSharpCode.SharpZipLib.Tar;

namespace SharpZipLibDeadLock
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      button.IsEnabled = false;

      //var fileContent = new byte[1000];
      //var tempFilename = "file.tar.gz";
      //System.Diagnostics.Trace.WriteLine(tempFilename);

      //using (var fs = new FileStream(tempFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
      //using (var gz = new GZipStream(fs, CompressionMode.Compress))
      //using (var tarWriter = new TarOutputStream(gz, Encoding.UTF8))
      //using (var contentStream = new MemoryStream(fileContent))
      //{
      //  for (var index = 0; index < 5; ++index)
      //  {
      //    var entry = TarEntry.CreateTarEntry($"file{index}");
      //    entry.Size = fileContent.Length;
      //    tarWriter.PutNextEntry(entry);
      //    contentStream.Seek(0, SeekOrigin.Begin);
      //    contentStream.CopyTo(tarWriter);
      //    tarWriter.CloseEntry();
      //  }
      //}

      var tarfilename = "SharpZipLibDeadLock.file.tar.gz";
      var tarfilestream = Assembly.GetExecutingAssembly().GetManifestResourceStream(tarfilename);
      using (var gz = new GZipStream(tarfilestream, CompressionMode.Decompress))
      using (var tarReader = new TarInputStream(gz, Encoding.UTF8))
      {
        foreach (var entryname in Files(tarReader))
          System.Diagnostics.Trace.WriteLine(entryname);
      }

      button.IsEnabled = true;
    }

    public IEnumerable<string> Files(TarInputStream tarReader)
    {
      TarEntry entry;
      while ((entry = tarReader.GetNextEntry()) != null)
      {
        if (entry.IsDirectory)
          continue;

        var name = entry.Name;
        yield return name;
      }
    }
  }
}
