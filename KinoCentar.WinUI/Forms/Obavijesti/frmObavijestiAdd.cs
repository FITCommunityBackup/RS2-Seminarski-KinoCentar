﻿using KinoCentar.Shared.Models;
using KinoCentar.Shared.Util;
using KinoCentar.WinUI.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KinoCentar.WinUI.Forms.Obavijesti
{
    public partial class frmObavijestiAdd : Form
    {
        private WebAPIHelper obavijestiService = new WebAPIHelper(Global.ApiAddress, Global.ObavijestiRoute);

        public frmObavijestiAdd()
        {
            InitializeComponent();
            this.AutoValidate = AutoValidate.Disable;
        }

        private void btnSnimi_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                var o = new ObavijestModel();
                o.Naslov = txtNaslov.Text;
                o.Tekst = txtTekst.Text;
                o.Datum = DateTime.Now;
                o.KorisnikId = Global.PrijavljeniKorisnik.Id;

                HttpResponseMessage response = obavijestiService.PostResponse(o).Handle();
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(Messages.add_obavijest_succ, Messages.msg_succ, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }

        private void btnOdustani_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(Messages.msg_cancel_que, Messages.msg_conf, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        #region Validation

        

        #endregion
    }
}
