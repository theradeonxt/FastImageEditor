//using System;
//using System.Drawing;
//using System.Drawing.Imaging;

//namespace ImageInterpolation
//{
//    class ParallelBitmap : IDisposable
//    {
//        private readonly BitmapData[] _dataBlocks;
//        private readonly Bitmap _sourceImg;

//        public ParallelBitmap(Bitmap img)
//        {
//            Blocks = Environment.ProcessorCount;

//            _sourceImg = img;

//            unsafe
//            {
//                _dataBlocks = new BitmapData[Blocks];
//                PartitionBegins = new byte*[Blocks];
//                PartitionDimensions = new uint[Blocks];

//                // Perform verical partitioning on the input image
//                // The image is split into horizontal chunks of equal sizes
//                int y = 0;
//                int h = _sourceImg.Height / Blocks;
//                for (int i = 0; i < Blocks; i++)
//                {
//                    /*_dataBlocks[i] = _sourceImg.LockBits(
//                        new Rectangle(0, y, _sourceImg.Width, h), ImageLockMode.ReadWrite, _sourceImg.PixelFormat);*/

//                    uint stride = (uint)Math.Abs(_dataBlocks[i].Stride);
//                    uint sizeBytes = (uint)(stride * y);

//                    PartitionBegins[i] = (byte*)(void*)_dataBlocks[i].Scan0;
//                    PartitionDimensions[i] = sizeBytes;

//                    y += (_sourceImg.Height / Blocks) % _sourceImg.Height;
//                }
//            }
//        }

//        public unsafe byte*[] PartitionBegins
//        {
//            get;
//            private set;
//        }


//        public uint[] PartitionDimensions
//        {
//            get;
//            private set;
//        }

//        public int Blocks
//        {
//            get;
//            private set;
//        }

//        public void Dispose()
//        {
//            for (int i = 0; i < Blocks; i++)
//            {
//                _sourceImg.UnlockBits(_dataBlocks[i]);
//            }
//        }
//    }
//}
