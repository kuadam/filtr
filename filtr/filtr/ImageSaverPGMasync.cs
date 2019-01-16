using System;
using System.IO;
using System.Text;

namespace filtr
{
	class ImageSaverPGMasync
	{
		public void save(string path, ImagePGMasync image)
		{
			var stream = File.Open(path, FileMode.OpenOrCreate);

			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("P2\n");
			stringBuilder.Append((image.Width - 2) + " " + (image.Height - 2) + "\n");
			stringBuilder.Append("255\n\n");

			for (int i = 1; i < image.Height - 1; i++)
			{
				for (int j = 1; j < image.Width - 1; j++)
				{
					stringBuilder.Append(Math.Floor(image.Pixels[j, i]) + " ");
				}
				stringBuilder.Append("\n");
			}

			stream.Write(Encoding.ASCII.GetBytes(stringBuilder.ToString()), 0, stringBuilder.Length);
			stream.Close();

		}
	}
}
