namespace filtr
{
	class ImagePGM
	{
		public int Height { get; }
		public int Width { get; }
		public float[,] Pixels { get; private set; }
		public ImagePGM()
		{
			Height = 1024;
			Width = 1024;
			Pixels = new float[Height, Width];
		}
		public void Generate()
		{
			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					if (j % 1024 < 512 && i % 1024 < 512 || (j % 1024 >= 512 && i % 1024 >= 512))
					{
						Pixels[j, i] = 255;
					}
					else
					{
						Pixels[j, i] = 0;
					}
				}
			}
		}

		public void Convolution()
		{
			ImagePGM newImagePgm = new ImagePGM();
			for (var i = 0; i < Width; i++)
			{
				for (var j = 0; j < Height; j++)
				{
					newImagePgm.Pixels[j, i] = Pixels[j, i] * 0.6f;
					if (j == 0)
					{
						newImagePgm.Pixels[j, i] += Pixels[j + 1, i] * 0.1f;
					}
					else if (j == Height - 1)
					{
						newImagePgm.Pixels[j, i] += Pixels[j - 1, i] * 0.1f;
					}
					else
					{
						newImagePgm.Pixels[j, i] += Pixels[j - 1, i] * 0.1f + Pixels[j + 1, i] * 0.1f;
					}
					
					if (i == 0)
					{
						newImagePgm.Pixels[j, i] += Pixels[j, i + 1] * 0.1f;
					}
					else if (i == Width - 1)
					{
						newImagePgm.Pixels[j, i] += Pixels[j, i - 1] * 0.1f;
					}
					else
					{
						newImagePgm.Pixels[j, i] += Pixels[j, i - 1] * 0.1f + Pixels[j, i + 1] * 0.1f;
					}
				}
			}

			Pixels = newImagePgm.Pixels;
		}
	}
}
