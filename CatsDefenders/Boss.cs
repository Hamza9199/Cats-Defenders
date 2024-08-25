using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace CatsDefenders
{
    public class Boss
    {
        public PictureBox BossPictureBox { get; private set; }
        public int Health { get; set; }

        public Boss(MainForm form)
        {
            Health = 20; // Boss ima 20 zdravlja

            // Kreiranje PictureBox-a za Bossa
            BossPictureBox = new PictureBox
            {
                Size = new Size(150, 150),
                Location = new Point(form.ClientSize.Width / 2 - 75, -150),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromFile(Path.Combine(form.basePath, "boss.jpg"))
            };

            form.Controls.Add(BossPictureBox);
        }

        // Funkcija za kretanje Bossa
        public void Kretanje(MainForm form)
        {
            BossPictureBox.Top += 2; // Boss se pomjera prema dolje

            // Provjera sudara sa igračem
            if (BossPictureBox.Bounds.IntersectsWith(form.igrac.Bounds))
            {
                form.zivoti -= 1;
                // Eventualna logika za Game Over
            }
        }

        // Funkcija za primanje štete
        public void PrimiStetu(MainForm form)
        {
            Health -= 1;
            if (Health <= 0)
            {
                form.Controls.Remove(BossPictureBox);
                form.boss = null;
                form.levelN++;
                form.PromijeniLevel(form.levelN); // Prelaz na sljedeći nivo
            }
        }
    }
}
