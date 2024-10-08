﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace CatsDefenders
{
	public static class IgraMeni
	{
		// Postavlja panel za pauzu igre
		public static void PostaviMenuPanel(MainForm form)
		{
			form.MenuPanel = new Panel
			{
				Size = new Size(300, 200),
				Location = new Point((form.ClientSize.Width - 300) / 2, (form.ClientSize.Height - 200) / 2),
				BackColor = Color.Black,
				Visible = false
			};

			form.menuText = new Label
			{
				Text = "Cats Defenders",
				ForeColor = Color.White,
				Font = new Font("Arial", 16, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(60, 30)
			};

			form.nastaviLabel = new Label
			{
				Text = "Nastavi",
				ForeColor = Color.White,
				Font = new Font("Arial", 14, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(70, 80),
				Cursor = Cursors.Hand
			};

			form.ugasiLabel = new Label
			{
				Text = "Izadji",
				ForeColor = Color.White,
				Font = new Font("Arial", 14, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(70, 110),
				Cursor = Cursors.Hand
			};

			form.MenuPanel.Controls.Add(form.menuText);
			form.MenuPanel.Controls.Add(form.nastaviLabel);
			form.MenuPanel.Controls.Add(form.ugasiLabel);
			form.Controls.Add(form.MenuPanel);

			form.nastaviLabel.Click += form.MenuClick;
			form.ugasiLabel.Click += form.UgasiLabel_Click;
		}

		// Postavlja panel za obavještenje o napretku na novi nivo
		public static void PostaviLevelUpPanel(MainForm form)
		{
			form.LevelUP = new Panel
			{
				Size = new Size(300, 200),
				Location = new Point((form.ClientSize.Width - 300) / 2, (form.ClientSize.Height - 200) / 2),
				BackColor = Color.Black,
				Visible = false
			};

			Label levelUpText = new Label
			{
				Text = "Level Up!",
				ForeColor = Color.White,
				Font = new Font("Arial", 16, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(60, 60)
			};

			Label label = new Label
			{
				Text = "Nastavi",
				ForeColor = Color.White,
				Font = new Font("Arial", 14, FontStyle.Bold),
				AutoSize = true,
				Location = new Point(70, 103),
				Cursor = Cursors.Hand
			};

			form.LevelUP.Controls.Add(levelUpText);
			form.LevelUP.Controls.Add(label);
			form.Controls.Add(form.LevelUP);

			label.Click += form.MenuClick;
		}
	}
}
