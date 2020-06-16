﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KinoCentar.Shared.Models;
using KinoCentar.Shared.Util;
using KinoCentar.WinUI.Util;
using KinoCentar.WinUI.Extensions;
using KinoCentar.WinUI.Forms.Filmovi;
using KinoCentar.WinUI.Forms.Projekcije;

namespace KinoCentar.WinUI.Forms.Rezervacije
{
    public partial class frmRezervacijeAdd : Form
    {
        private WebAPIHelper rezervacijeService = new WebAPIHelper(Global.ApiAddress, Global.RezervacijeRoute);
        private WebAPIHelper projekcijeService = new WebAPIHelper(Global.ApiAddress, Global.ProjekcijeRoute);
        private WebAPIHelper korisniciService = new WebAPIHelper(Global.ApiAddress, Global.KorisniciRoute);

        public frmRezervacijeAdd()
        {
            InitializeComponent();
            this.AutoValidate = AutoValidate.Disable;
        }

        private void frmRezervacijeAdd_Load(object sender, EventArgs e)
        {
            var projekcijeResponse = projekcijeService.GetResponse().Handle();
            if (projekcijeResponse.IsSuccessStatusCode)
            {
                var projekcije = projekcijeResponse.GetResponseResult<List<ProjekcijaModel>>();
                cmbProjekcija.DataSource = projekcije;
                cmbProjekcija.DisplayMember = "FilmNaslov";
                cmbProjekcija.ValueMember = "Id";
            }

            var korisnikResponse = korisniciService.GetResponse().Handle();
            if (korisnikResponse.IsSuccessStatusCode)
            {
                var korisnici = korisnikResponse.GetResponseResult<List<KorisnikModel>>();
                korisnici.Insert(0, new KorisnikModel());
                cmbKorisnik.DataSource = korisnici;
                cmbKorisnik.DisplayMember = "ImePrezime";
                cmbKorisnik.ValueMember = "Id";
            }
        }

        private void btnSnimi_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                var r = new RezervacijaModel();
                var projekcija = (ProjekcijaModel)cmbProjekcija.SelectedItem;

                r.ProjekcijaId = projekcija.Id;
                if (cmbKorisnik.SelectedIndex != 0)
                {
                    r.KorisnikId = ((KorisnikModel)cmbKorisnik.SelectedItem).Id;
                }
                if (cmbBrojSjedista.SelectedIndex != 0)
                {
                    r.BrojSjedista = Convert.ToInt32(cmbBrojSjedista.SelectedItem);
                }

                r.DatumProjekcije = dtpDatumProjekcije.Value;
                r.Cijena = projekcija.Cijena;
                r.Datum = DateTime.Now;

                HttpResponseMessage response = rezervacijeService.PostResponse(r).Handle();
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(Messages.add_rezervacija_succ, Messages.msg_succ, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnProjekcijaInfo_Click(object sender, EventArgs e)
        {
            try
            {
                var projekcijaId = ((ProjekcijaModel)cmbProjekcija.SelectedItem).Id;
                var frm = new frmProjekcijeEdit(projekcijaId);
                frm.ShowDialog();
            }
            catch
            { }
        }

        private void cmbProjekcija_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var projekcija = (ProjekcijaModel)cmbProjekcija.SelectedItem;

                dtpDatumProjekcije.MinDate = projekcija.VrijediOd;
                dtpDatumProjekcije.MaxDate = projekcija.VrijediDo;

                var retSjedistaResponse = rezervacijeService.GetActionResponse("FreeSeats", projekcija.Id.ToString()).Handle();
                if (retSjedistaResponse.IsSuccessStatusCode)
                {
                    var retSjedista = retSjedistaResponse.GetResponseResult<List<int>>();
                    cmbBrojSjedista.DataSource = retSjedista;
                }
            }
            catch
            { }            
        }

        #region Validation



        #endregion
    }
}