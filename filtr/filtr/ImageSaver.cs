using System;
using System.IO;
using System.Text;

namespace filtr
{
	class ImageSaver
	{
		public void save(string path, ImagePGM image)
		{
			var stream = File.Open(path, FileMode.OpenOrCreate);

			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("P2\n");
			stringBuilder.Append((image.Width) + " " + (image.Height) + "\n");
			stringBuilder.Append("255\n\n");

			for (int i = 0; i < image.Height; i++)
			{
				for (int j = 0; j < image.Width; j++)
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
