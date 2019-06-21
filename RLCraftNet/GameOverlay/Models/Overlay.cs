using System;
using System.Drawing;

namespace GameOverlay.Models
{
    public class Overlay
    {
        // X and Y are absolute coordinates where the overlay window begins (top-left point).
        public Overlay(int x, int y, int width, int height)
        {
            BaseOverlayTop = y;
            BaseOverlayLeft = x;
            BaseOverlayWidth = width;
            BaseOverlayHeight = height;

            SelfFrame = new PartyFrame(BaseOverlayLeft, BaseOverlayTop, 50, 500);
            TankFrame = new PartyFrame(BaseOverlayLeft, BaseOverlayTop, 50, 600);
            DpsFrames[0] = new PartyFrame(BaseOverlayLeft, BaseOverlayTop, 50, 700);
            DpsFrames[1] = new PartyFrame(BaseOverlayLeft, BaseOverlayTop, 50, 800);
            DpsFrames[2] = new PartyFrame(BaseOverlayLeft, BaseOverlayTop, 50, 900);
        }

        // Base overlay coordinates
        private int BaseOverlayTop { get; set; }
        private int BaseOverlayLeft { get; set; }
        private int BaseOverlayWidth { get; set; }
        private int BaseOverlayHeight { get; set; }

        // Derived coordinates
        public PartyFrame SelfFrame { get; set; }
        public PartyFrame TankFrame { get; set; }
        public PartyFrame DpsFrame1 { get; set; }
        public PartyFrame DpsFrame2 { get; set; }
        public PartyFrame DpsFrame3 { get; set; }
        public PartyFrame[] DpsFrames { get; set; } = new PartyFrame[3];
    }

    public class PartyFrame
    {
        private const int HEALTH_ARRAY_LENGTH = 20;
        private const int HEALTH_RECT_MARGIN_PX = 10;

        public PartyFrame(int baseX, int baseY, int offsetX, int offsetY)
        {
            this.BaseX = baseX;
            this.BaseY = baseY;

            for (int i = 0; i < 20; i++)
            {
                Health[i].X = baseX + offsetX + 17*i;
                Health[i].Y = baseY + offsetY;
                Mana[i].X = baseX + offsetX + 17*i;
                Mana[i].Y = baseY + offsetY + 10;
            }
        }

        private int BaseX { get; set; }
        private int BaseY { get; set; }

        // Point.X and Point.Y are absolute coordinates based off the monitor resolution while
        // Overlay coordinates are relative coordinates based off the overlay window.
        public Point[] Health { get; set; } = new Point[HEALTH_ARRAY_LENGTH];
        public Point[] Mana { get; set; } = new Point[HEALTH_ARRAY_LENGTH];
        public Point[] CastBar { get; set; } = new Point[HEALTH_ARRAY_LENGTH];

        // Returns a rectangle surrounding surrounding the pixels representing the health bar.
        public Rectangle GetRect()
        {
            return new Rectangle(
                Health[0].X - BaseX, 
                Health[0].Y - BaseY - HEALTH_RECT_MARGIN_PX, 
                Health[HEALTH_ARRAY_LENGTH - 1].X - Health[0].X,
                HEALTH_RECT_MARGIN_PX);
        }
    }
}
