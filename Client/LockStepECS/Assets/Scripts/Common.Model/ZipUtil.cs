using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using Lockstep.Util;

// Token: 0x02000009 RID: 9
public class ZipUtil : object
{
	// Token: 0x06000011 RID: 17 RVA: 0x00002670 File Offset: 0x00000870
	public static void ZipDir(string dir, string descFile, string exts = "*.*", int compression = 4)
	{
		List<string> files = new List<string>();
		PathUtil.Walk(dir, exts, delegate(string path)
		{
			files.Add(path);
		}, false, true);
		ZipUtil.ZipFiles(files, descFile, compression);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000026B4 File Offset: 0x000008B4
	public static void ZipFiles(List<string> sourceFileLists, string descFile, int compression = 4)
	{
		ZipConstants.DefaultCodePage = 65001;
		bool flag = compression < 0 || compression > 9;
		if (flag)
		{
			throw new ArgumentException("Error compress Level");
		}
		bool flag2 = !Directory.Exists(Path.GetDirectoryName(descFile));
		if (flag2)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(descFile));
		}
		foreach (string text in sourceFileLists)
		{
			bool flag3 = !File.Exists(text);
			if (flag3)
			{
				throw new ArgumentException(string.Format("File {0} not exist！", text));
			}
		}
		Crc32 crc = new Crc32();
		using (ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(descFile)))
		{
			zipOutputStream.SetLevel(compression);
			for (int i = 0; i < sourceFileLists.Count; i++)
			{
				ZipEntry zipEntry = new ZipEntry(Path.GetFileName(sourceFileLists[i]));
				zipEntry.DateTime = DateTime.Now;
				using (FileStream fileStream = File.OpenRead(sourceFileLists[i]))
				{
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, array.Length);
					zipEntry.Size = fileStream.Length;
					crc.Reset();
					crc.Update(array);
					zipEntry.Crc = crc.Value;
					zipOutputStream.PutNextEntry(zipEntry);
					zipOutputStream.Write(array, 0, array.Length);
				}
				zipOutputStream.CloseEntry();
			}
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002880 File Offset: 0x00000A80
	public static void UnZip(string sourceFile, string path)
	{
		ZipConstants.DefaultCodePage = 65001;
		bool flag = !File.Exists(sourceFile);
		if (flag)
		{
			throw new ArgumentException("File not exist! " + sourceFile);
		}
		bool flag2 = !Directory.Exists(path);
		if (flag2)
		{
			Directory.CreateDirectory(path);
		}
		ZipFile zipFile = new ZipFile(sourceFile);
		byte[] array = new byte[2048];
		foreach (object obj in zipFile)
		{
			ZipEntry zipEntry = (ZipEntry)obj;
			Stream inputStream = zipFile.GetInputStream(zipEntry);
			using (FileStream fileStream = File.Create(Path.Combine(path, zipEntry.Name)))
			{
				for (;;)
				{
					int num = inputStream.Read(array, 0, array.Length);
					bool flag3 = num > 0;
					if (!flag3)
					{
						break;
					}
					fileStream.Write(array, 0, num);
				}
			}
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000029A8 File Offset: 0x00000BA8
	public static string Zip(string text)
	{
		ZipConstants.DefaultCodePage = 65001;
		string result = string.Empty;
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] array = ZipUtil.Zip(bytes, false);
		result = Convert.ToBase64String(array);
		Array.Clear(array, 0, array.Length);
		return result;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000029F4 File Offset: 0x00000BF4
	public static byte[] Zip(byte[] data, bool isClearData = false)
	{
		ZipConstants.DefaultCodePage = 65001;
		byte[] result = null;
		Deflater deflater = new Deflater(9);
		deflater.SetInput(data);
		deflater.Finish();
		using (MemoryStream memoryStream = new MemoryStream(data.Length))
		{
			byte[] array = new byte[2048];
			while (!deflater.IsFinished)
			{
				int count = deflater.Deflate(array);
				memoryStream.Write(array, 0, count);
			}
			result = memoryStream.ToArray();
		}
		if (isClearData)
		{
			Array.Clear(data, 0, data.Length);
		}
		return result;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002AA8 File Offset: 0x00000CA8
	public static string UnZip(string text)
	{
		string result = string.Empty;
		byte[] data = Convert.FromBase64String(text);
		byte[] array = ZipUtil.UnZip(data, true);
		result = Encoding.UTF8.GetString(array);
		Array.Clear(array, 0, array.Length);
		return result;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002AE8 File Offset: 0x00000CE8
	public static byte[] UnZip(byte[] data, bool isClearData = true)
	{
		byte[] result = null;
		Inflater inflater = new Inflater();
		inflater.SetInput(data);
		int num = 0;
		using (MemoryStream memoryStream = new MemoryStream(data.Length))
		{
			byte[] array = new byte[2048];
			while (!inflater.IsFinished)
			{
				num = inflater.Inflate(array);
				memoryStream.Write(array, 0, num);
			}
			result = memoryStream.ToArray();
		}
		if (isClearData)
		{
			Array.Clear(data, 0, num);
		}
		return result;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002B84 File Offset: 0x00000D84
	public static string GZipCompress(string text)
	{
		string result = string.Empty;
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] array = ZipUtil.GZipCompress(bytes, true);
		result = Convert.ToBase64String(array);
		Array.Clear(array, 0, array.Length);
		return result;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002BC4 File Offset: 0x00000DC4
	public static string GZipDeCompress(string text)
	{
		string result = string.Empty;
		byte[] data = Convert.FromBase64String(text);
		byte[] array = ZipUtil.GZipDeCompress(data, true);
		result = Encoding.UTF8.GetString(array);
		Array.Clear(array, 0, array.Length);
		return result;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002C04 File Offset: 0x00000E04
	public static byte[] GZipCompress(byte[] data, bool isClearData = true)
	{
		byte[] result = null;
		try
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (Stream stream = new GZipOutputStream(memoryStream))
				{
					stream.Write(data, 0, data.Length);
					stream.Flush();
				}
				result = memoryStream.ToArray();
			}
		}
		catch (SharpZipBaseException)
		{
		}
		catch (IndexOutOfRangeException)
		{
		}
		if (isClearData)
		{
			Array.Clear(data, 0, data.Length);
		}
		return result;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002CB4 File Offset: 0x00000EB4
	public static byte[] GZipDeCompress(byte[] data, bool isClearData = true)
	{
		byte[] result = null;
		try
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (MemoryStream memoryStream2 = new MemoryStream(data))
				{
					using (Stream stream = new GZipInputStream(memoryStream2))
					{
						stream.Flush();
						byte[] array = new byte[2048];
						int count;
						while ((count = stream.Read(array, 0, array.Length)) > 0)
						{
							memoryStream.Write(array, 0, count);
						}
					}
				}
				result = memoryStream.ToArray();
			}
		}
		catch (SharpZipBaseException)
		{
		}
		catch (IndexOutOfRangeException)
		{
		}
		if (isClearData)
		{
			Array.Clear(data, 0, data.Length);
		}
		return result;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002DB4 File Offset: 0x00000FB4
	public static string TarCompress(string text)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] array = ZipUtil.TarCompress(bytes, true);
		string result = Convert.ToBase64String(array);
		Array.Clear(array, 0, array.Length);
		return result;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002DF0 File Offset: 0x00000FF0
	public static string TarDeCompress(string text)
	{
		byte[] data = Convert.FromBase64String(text);
		byte[] array = ZipUtil.TarDeCompress(data, true);
		string @string = Encoding.UTF8.GetString(array);
		Array.Clear(array, 0, array.Length);
		return @string;
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002E2C File Offset: 0x0000102C
	public static byte[] TarCompress(byte[] data, bool isClearData = true)
	{
		byte[] result = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (Stream stream = new TarOutputStream(memoryStream))
			{
				stream.Write(data, 0, data.Length);
				stream.Flush();
			}
			result = memoryStream.ToArray();
		}
		if (isClearData)
		{
			Array.Clear(data, 0, data.Length);
		}
		return result;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002EB4 File Offset: 0x000010B4
	public static byte[] TarDeCompress(byte[] data, bool isClearData = true)
	{
		byte[] result = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (MemoryStream memoryStream2 = new MemoryStream(data))
			{
				using (Stream stream = new TarInputStream(memoryStream2))
				{
					stream.Flush();
					byte[] array = new byte[2048];
					int count;
					while ((count = stream.Read(array, 0, array.Length)) > 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
			}
			result = memoryStream.ToArray();
		}
		if (isClearData)
		{
			Array.Clear(data, 0, data.Length);
		}
		return result;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002F8C File Offset: 0x0000118C
	public static string BZipCompress(string text)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] array = ZipUtil.BZipCompress(bytes, true);
		string result = Convert.ToBase64String(array);
		Array.Clear(array, 0, array.Length);
		return result;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002FC8 File Offset: 0x000011C8
	public static string BZipDeCompress(string text)
	{
		byte[] data = Convert.FromBase64String(text);
		byte[] array = ZipUtil.BZipDeCompress(data, true);
		string @string = Encoding.UTF8.GetString(array);
		Array.Clear(array, 0, array.Length);
		return @string;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00003004 File Offset: 0x00001204
	public static byte[] BZipCompress(byte[] data, bool isClearData = true)
	{
		byte[] result = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (Stream stream = new BZip2OutputStream(memoryStream))
			{
				stream.Write(data, 0, data.Length);
				stream.Flush();
			}
			result = memoryStream.ToArray();
		}
		if (isClearData)
		{
			Array.Clear(data, 0, data.Length);
		}
		return result;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x0000308C File Offset: 0x0000128C
	public static byte[] BZipDeCompress(byte[] data, bool isClearData = true)
	{
		byte[] result = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (MemoryStream memoryStream2 = new MemoryStream(data))
			{
				using (Stream stream = new BZip2InputStream(memoryStream2))
				{
					stream.Flush();
					byte[] array = new byte[2048];
					int count;
					while ((count = stream.Read(array, 0, array.Length)) > 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
			}
			result = memoryStream.ToArray();
		}
		if (isClearData)
		{
			Array.Clear(data, 0, data.Length);
		}
		return result;
	}

	// Token: 0x04000002 RID: 2
	private const int BUFFER_LENGTH = 2048;
}
