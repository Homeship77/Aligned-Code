using System;

namespace UnityStandardAssets.Utility
{
    public struct FastRandom
	{	
		const double REAL_UNIT_INT = 1.0/((double)int.MaxValue+1.0);
		const double REAL_UNIT_UINT = 1.0/((double)uint.MaxValue+1.0);
		const uint Y=842502087, Z=3579807591, W=273326509;

		uint x, y, z, w;

		public FastRandom(int seed):this()
		{
			SetSeed(seed);
		}

        static int seedOffset;
		public void SetByTicks()
        {
            SetSeed((int)Environment.TickCount + seedOffset);

            seedOffset++;
        }

		public void SetSeed(int seed)
		{
            var tmp = (uint)seed;
			x = tmp << 16 | tmp >> 16;
			y = Y;
			z = Z;
			w = W;
		}


		public int Next()
		{
			uint t=(x^(x<<11));
			x=y; y=z; z=w;
			w=(w^(w>>19))^(t^(t>>8));

			uint rtn = w&0x7FFFFFFF;
			if(rtn==0x7FFFFFFF)
				return Next();
			return (int)rtn;			
		}

		public int Next(int upperBound)
		{
			if(upperBound<0)
				throw new ArgumentOutOfRangeException("upperBound", upperBound, "upperBound must be >=0");

			uint t=(x^(x<<11));
			x=y; y=z; z=w;

			return (int)((REAL_UNIT_INT*(int)(0x7FFFFFFF&(w=(w^(w>>19))^(t^(t>>8)))))*upperBound);
		}

		
		public int Range(int min, int max)
		{
			if (min >= max)
			{
				if (min == max) 
					return min;

				throw new ArgumentOutOfRangeException("upperBound", max, "upperBound must be >=lowerBound");
			}

			uint t=(x^(x<<11));
			x=y; y=z; z=w;

			int range = max-min;
			if(range<0)
			{
				return min+(int)((REAL_UNIT_UINT*(double)(w=(w^(w>>19))^(t^(t>>8))))*(double)((long)max-(long)min));
			}
            
			return min+(int)((REAL_UNIT_INT* (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ t ^ (t >> 8)))) * range);
		}

        public float Range(float min, float max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException("upperBound", max, "upperBound must be >=lowerBound");

            float range = max - min;

            return min + (float)(range * NextDouble());
        }

		public double NextDouble()
		{	
			uint t=(x^(x<<11));
			x=y; y=z; z=w;

			return (REAL_UNIT_INT*(int)(0x7FFFFFFF&(w=(w^(w>>19))^(t^(t>>8)))));			
		}

        public float NextFloat()
        {
            return (float)NextDouble();
        }

		
		public void NextBytes(byte[] buffer)
		{	
			uint x=this.x, y=this.y, z=this.z, w=this.w;
			int i=0;
			uint t;
			for(int bound=buffer.Length-3; i<bound;)
			{	
				t=(x^(x<<11));
				x=y; y=z; z=w;
				w=(w^(w>>19))^(t^(t>>8));

				buffer[i++] = (byte)w;
				buffer[i++] = (byte)(w>>8);
				buffer[i++] = (byte)(w>>16);
				buffer[i++] = (byte)(w>>24);
			}

			if(i<buffer.Length)
			{	
				t=(x^(x<<11));
				x=y; y=z; z=w;
				w=(w^(w>>19))^(t^(t>>8));

				buffer[i++] = (byte)w;
				if(i<buffer.Length)
				{
					buffer[i++]=(byte)(w>>8);
					if(i<buffer.Length)
					{	
						buffer[i++] = (byte)(w>>16);
						if(i<buffer.Length)
						{	
							buffer[i] = (byte)(w>>24);
						}
					}
				}
			}
			this.x=x; this.y=y; this.z=z; this.w=w;
		}


		public uint NextUInt()
		{
			uint t=(x^(x<<11));
			x=y; y=z; z=w;
			return (w=(w^(w>>19))^(t^(t>>8)));
		}

		
		public int NextInt()
		{
			uint t=(x^(x<<11));
			x=y; y=z; z=w;
			return (int)(0x7FFFFFFF&(w=(w^(w>>19))^(t^(t>>8))));
		}

	}
}
