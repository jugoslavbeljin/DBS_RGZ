using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; 
namespace kontrolaBaze
{
    public partial class frmMain : Form
    {

        String connString_ = "";

        public frmMain()
        {
            InitializeComponent();
        }

        private void btn_konekcija_Click(object sender, EventArgs e)
        {
            //idemo redom sta mi treba
            //Server=myServerAddress;Port=1234;Database=myDataBase;Uid=myUsername;Pwd = myPassword;

            if (txt_sreverName.Text=="")
            {
                MessageBox.Show("Morate uneti naziv servera");
                return; 
            }

            if (txt_DatabaseName.Text == "")
            {
                MessageBox.Show("Morate uneti naziv baze");
                return;
            }

            if (txt_userName.Text == "")
            {
                MessageBox.Show("Morate uneti korisnicko ime");
                return;
            }

            if (txt_password.Text == "")
            {
                MessageBox.Show("Morate uneti lozinku");
                return;
            }

            connString_ = "Server=" + txt_sreverName.Text + "; Port=3306; Database=" + txt_DatabaseName.Text + "; Uid=" + txt_userName.Text + "; Pwd=" + txt_password.Text;

            //sada mozemo da idemo dalje

            MySqlConnection conn_ = new MySqlConnection(connString_);
            try
            {
                conn_.Open();
                //ako je uspeo to je ok
                conn_.Close();
                conn_ = null;
                lbl_porukaKonekcija.Text = "Konekcija uspesno izvrsena na bazu: " + txt_DatabaseName.Text ;
                kontrolaToolStripMenuItem.Enabled = true;
            }
            catch
            {
                lbl_porukaKonekcija.Text = "Imate problem sa konekcijom proverite parametre";
                kontrolaToolStripMenuItem.Enabled = false;
            }

        }

        private void jEToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //ok sada mozemo da idemo query po query
            txtRezultatKontrole.Text = DateTime.Now.ToString() ;

            //ok aj prvo da napravimo dve matrice u jednoj je sql a u 
            //drugoj je komentar odnosno naziv kontrole

                   
            MySqlConnection con_ = new MySqlConnection(connString_);
            MySqlDataReader reader_;
            

            try
            {
                con_.Open();
            } catch
            {
                MessageBox.Show("Problem sa konkcijom na bazu");
                return;
            }

            Int32 brojimDo_ = 72;

            pb1.Maximum = brojimDo_;

            MySqlCommand com_ = new MySqlCommand("", con_);
           
            Int32 i;         

            for (i=0; i<= brojimDo_; i++)
            {
                try
                {
                    con_.Open();
                }
                catch (Exception ex)
                {
                }
                
                String[] izlaz_= new string[1];
                izlaz_ = getSQLComment(i);

                com_.CommandText = izlaz_[0];
                txtRezultatKontrole.Text=txtRezultatKontrole.Text + System.Environment.NewLine + izlaz_[1] + System.Environment.NewLine;
                int brojac_ = 0;
                try
                {
                    reader_ = com_.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader_.HasRows)
                    {
                        //sada treba videti sta se stampa
                        while (reader_.Read())
                        {
                            try
                            {
                                txtRezultatKontrole.Text = txtRezultatKontrole.Text + reader_.GetName(0) + "=" + reader_.GetValue(0) + reader_.GetName(1) + "=" + reader_.GetValue(1) + reader_.GetName(2) + "=" + reader_.GetValue(2) + System.Environment.NewLine;
                            }
                            catch
                            {
                                txtRezultatKontrole.Text = txtRezultatKontrole.Text + reader_.GetName(0) + "=" + reader_.GetValue(0) + reader_.GetName(1) + "=" + reader_.GetValue(1) ;
                            }
                            brojac_ += 1;
                           if (brojac_>500)
                            {
                                txtRezultatKontrole.Text = txtRezultatKontrole.Text + System.Environment.NewLine + "Sistematski problem preko 500 gresaka" + System.Environment.NewLine;
                                break;
                            }
                        }
                        txtRezultatKontrole.Text = txtRezultatKontrole.Text + System.Environment.NewLine + com_.CommandText + System.Environment.NewLine;
                    }
                    else
                    {
                        //znaci prosli
                        txtRezultatKontrole.Text = txtRezultatKontrole.Text + "    nema gresaka" + System.Environment.NewLine;
                    }
                    reader_.Close();

                }
                catch (Exception ex)
                {
                   
                    txtRezultatKontrole.Text = txtRezultatKontrole.Text + ex.Message;
                   
                }

                
                pb1.Value = i;
            }

            txtRezultatKontrole.Text = txtRezultatKontrole.Text + System.Environment.NewLine + DateTime.Now.ToString();
            MessageBox.Show("Kraj kontrole");
            pb1.Value = 0;

        }

        private void izvestajUTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sf_diag.FileName = "";
            sf_diag.ShowDialog();

            if (sf_diag.FileName == "")
            {
                MessageBox.Show("Morate izabrati file");
                return;
            }
            String[] lines = { txtRezultatKontrole.Text };
            
            System.IO.File.WriteAllLines(sf_diag.FileName, lines);


        }

        private void jEUFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ok sada mozemo da idemo query po query
            txtRezultatKontrole.Text = DateTime.Now.ToString();

            //ok aj prvo da napravimo dve matrice u jednoj je sql a u 
            //drugoj je komentar odnosno naziv kontrole

            sf_diag.FileName="";
            sf_diag.ShowDialog();

            if (sf_diag.FileName=="")
            {
                Close();
            }

            
            MySqlConnection con_ = new MySqlConnection(connString_);
            MySqlDataReader reader_;


            try
            {
                con_.Open();
            }
            catch
            {
                MessageBox.Show("Problem sa konkcijom na bazu");
                return;
            }

            Int32 brojimDo_ = 72;

            pb1.Maximum = brojimDo_;

            MySqlCommand com_ = new MySqlCommand("", con_);
            // KONTROLA PAR BAZE
           
            Int32 i;

            using (System.IO.StreamWriter file= new System.IO.StreamWriter(sf_diag.FileName))
            {

            
            for (i = 0; i <= brojimDo_; i++)
            {
                try
                {
                    con_.Open();
                }
                catch (Exception ex)
                {
                }

                    String[] izlaz_ = new string[1];
                    izlaz_ = getSQLComment(i);

                    com_.CommandText = izlaz_[0];
                    txtRezultatKontrole.Text = txtRezultatKontrole.Text + System.Environment.NewLine + izlaz_[1] + System.Environment.NewLine;

                file.WriteLine(System.Environment.NewLine + izlaz_[1] + System.Environment.NewLine);

                int brojac_ = 0;

                try
                {
                    reader_ = com_.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader_.HasRows)
                    {
                        //sada treba videti sta se stampa
                        while (reader_.Read())
                        {
                            try
                            {
                                file.WriteLine(reader_.GetName(0) + "=" + reader_.GetValue(0) + reader_.GetName(1) + "=" + reader_.GetValue(1) + reader_.GetName(2) + "=" + reader_.GetValue(2) + System.Environment.NewLine);
                            }
                            catch
                            {
                                    file.WriteLine(reader_.GetName(0) + "=" + reader_.GetValue(0) + reader_.GetName(1) + "=" + reader_.GetValue(1));
                            }
                            brojac_ += 1;
                            if (brojac_ > 500)
                            {
                                    file.WriteLine(System.Environment.NewLine + "Sistematski problem preko 500 gresaka" + System.Environment.NewLine);
                                break;
                            }
                        }
                            file.WriteLine(System.Environment.NewLine + com_.CommandText + System.Environment.NewLine);
                    }
                    else
                    {
                            //znaci prosli
                            file.WriteLine("    nema gresaka" + System.Environment.NewLine);
                    }
                    reader_.Close();

                }
                catch (Exception ex)
                {

                        file.WriteLine(ex.Message);

                }


                pb1.Value = i;
            }

                file.WriteLine(System.Environment.NewLine + DateTime.Now.ToString());

            }

            txtRezultatKontrole.Text = DateTime.Now.ToString() + System.Environment.NewLine  + txtRezultatKontrole.Text;

            MessageBox.Show("Kraj kontrole");
            pb1.Value = 0;

        }

        private void updateNekeDefaultPoljaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySqlConnection con_ = new MySqlConnection(connString_);
            

            try
            {
                con_.Open();
            }
            catch
            {
                MessageBox.Show("Problem sa konkcijom na bazu");
                return;
            }

            pb1.Maximum = 53;

            MySqlCommand com_ = new MySqlCommand("", con_);

            String[] sqlMat = new String[100];



            sqlMat[0] = "update je_kor set imeoca = '' where imeoca is null;";
            sqlMat[1] = "update je_kor set ime = '' where ime is null;";
            sqlMat[2] = "update je_kor set devprez = '' where devprez is null;";
            sqlMat[3] =" update je_kor set imemuza = '' where imemuza is null;";
            sqlMat[4] = "update je_kor set broj = '' where broj is null;";
            // ovde ide vli baza
            sqlMat[5] = "update je_vli set zku='' where zku is null";
            sqlMat[6] = "update je_vli set zk_br='' where zk_br is null";
            sqlMat[7] = "update je_vli set sprat='' where sprat is null";
            sqlMat[8] = "update je_vli set brojstana='' where brojstana is null";
            sqlMat[9] = "update je_vli set struktura='' where struktura is null";
            sqlMat[10] = "update je_vli set slovo='' where slovo is null";
            sqlMat[11] = "update je_vli set ulaz='' where ulaz is null";
            sqlMat[12] = "update je_vli set etaza1='' where etaza1 is null";
            sqlMat[13] = "update je_vli set etaza2='' where etaza2 is null";
            sqlMat[14] = "update je_vli set etaza3='' where etaza3 is null";
            sqlMat[15] = "update je_vli set etaza4='' where etaza4 is null";
            sqlMat[16] = "update je_vli set nacinpovr='' where nacinpovr is null";
            sqlMat[17] = "update je_vli set broj='' where broj is null";
            sqlMat[18] = "update je_vli set uzbroj='' where uzbroj is null";
            sqlMat[19] = "update je_vli set imenilac=right(concat('000000',imenilac),6);";
            sqlMat[20] = "update je_vli set brojilac=right(concat('000000',brojilac),6);";
            sqlMat[21] = "update je_vli set brojparc=right(concat('000000',brojparc),5);";
            sqlMat[22] = "update je_vli set podbroj=right(concat('000000',podbroj),3);";
            sqlMat[46] = "update je_vli set prezime='' where prezime is null";
            sqlMat[46] = "update je_vli set ime='' where ime is null";
            sqlMat[47] = "update je_vli set imeoca='' where imeoca is null";
            sqlMat[48] = "update je_vli set adresa='' where adresa is null";

            //sada idemo na par - mada je to trebalo na pocetku al aj :)
            sqlMat[23]=  "update je_par set brojparc=right(concat('000000',brojparc),5);";
            sqlMat[24] = "update je_par set podbroj=right(concat('000000',podbroj),3);";
            sqlMat[25] = "update je_par set brposlis=right(concat('000000',brposlis),5);";
            sqlMat[26] = "update je_par set skica='' where skica is null";
            sqlMat[27] = "update je_par set manual ='' where manual is null";
            sqlMat[28] = "update je_par set god='' where god is null";
            sqlMat[29] = "update je_par set broj='' where broj is null";
            sqlMat[41] = "update je_par set uzbroj='' where uzbroj is null";
            sqlMat[42] = "update je_par set sulice=right(concat('000000',sulice),5);";
            sqlMat[43] = "update je_par set hektari=right(concat('000000',hektari),5);";
            sqlMat[44] = "update je_par set ari=right(concat('000000',ari),2);";
            sqlMat[45] = "update je_par set metri=right(concat('000000',metri),2);";

            //je_kor nastavak
            sqlMat[30] = "update je_kor set brposlis=right(concat('000000',brposlis),5);";
            sqlMat[31] = "update je_kor set uzbroj = '' where uzbroj is null;";
            sqlMat[32] = "update je_kor set imenilac=right(concat('000000',imenilac),6);";
            sqlMat[33] = "update je_kor set brojilac=right(concat('000000',brojilac),6);";

            //je_gli 
            sqlMat[34] = "update je_gli set brojparc=right(concat('000000',brojparc),5);";
            sqlMat[35] = "update je_gli set podbroj=right(concat('000000',podbroj),3);";
            sqlMat[36] = "update je_gli set zku='' where zku is null";
            sqlMat[41] = "update je_gli set zku=right(concat('000',zku),3) where zku<>''";
            sqlMat[37] = "update je_gli set zk_br='' where zk_br is null";
            sqlMat[38] = "update je_gli set sprat='' where sprat is null";
            sqlMat[39] = "update je_gli set brojstana='' where brojstana is null";
            sqlMat[40] = "update je_gli set slovo='' where slovo is null";

            sqlMat[49] = "update je_vli set ETAZA1 = '', ETAZA2 = '', ETAZA3 = '', ETAZA4 = '' where NACINKOR in (3001, 3002, 3003) AND(ETAZA1 <> 0 or ETAZA2 <> 0 or ETAZA3 <> 0 or ETAZA4 <> 0)";
            sqlMat[50] = "update je_vli set evidencija=1 where BROJSTANA<>0 and EVIDENCIJA=0";
            sqlMat[51] = "update je_vli set OBIMPRAVA=4 where instr(BROJILAC,'Z')<>4";
            sqlMat[52] = "UPDATE JE_VLI SET OBIMPRAVA=1 WHERE BROJILAC='000001' AND IMENILAC='000001'";

            Int32 i;
            
            for (i = 0; i < 53; i++)
                {
                try
                {
                    com_.CommandText = sqlMat[i];
                    com_.ExecuteNonQuery();
                    pb1.Value = i;
                }

                    catch (MySqlException ex)
                {
                    txtRezultatKontrole.Text = System.Environment.NewLine + txtRezultatKontrole.Text + System.Environment.NewLine + ex.Message ;

                }
                    

                }
            pb1.Value = 0;
            MessageBox.Show("Kraj");

        }

        static String[] getSQLComment(Int32 gde_)
        {
            String[] sqlMat = new String[100];
            String[] commentarMat = new String[100];

            sqlMat[0] = "select * FROM je_par where cast(brojparc as UNSIGNED)=0 or (cast(podbroj as UNSIGNED)=0 and podbroj<>\"000\") or brojparc is null or podbroj is null or length(brojparc)<>5 or length(podbroj)<>3";
            sqlMat[1] = "select * FROM je_par where brposlis is null or cast(brposlis as UNSIGNED)=0 or LENGTH(brposlis)<>5";
            sqlMat[2] = "SELECT * FROM je_par where (skica=\"\" or skica is null) and (cast(god as unsigned) <> 0) or brojplana is null or brojplana=\"\"";
            sqlMat[3] = "SELECT * FROM ( SELECT brojparc, podbroj, NAZIVULICE as naziv, je_par.sulice, 'ulice' FROM je_par LEFT OUTER JOIN je_ulice ON je_par.sulice = je_ulice.sulice WHERE ulicapotes = 1 ) AS A WHERE NAZIV IS NULL UNION SELECT * FROM ( SELECT brojparc, podbroj, nazivpotesa as NAZIV, je_par.sulice, 'potes' FROM je_par LEFT OUTER JOIN je_potesi ON je_par.sulice = je_potesi.spotesa WHERE ulicapotes = 9 ) AS A WHERE NAZIV IS NULL";
            sqlMat[4] = "SELECT * from je_par where length(hektari)<>5 and length(ari)<>3 and length(metri)<>3 AND (cast(hektari as UNSIGNED)*10000+cast(ari as UNSIGNED)*100+cast(metri as UNSIGNED) )<=0";
            sqlMat[5] = "SELECT * FROM (SELECT je_par.*,NAZIV FROM je_par LEFT OUTER JOIN kat_gradjevi on je_par.ggradjzem=kat_gradjevi.SIFRA ) A where naziv is null";
            sqlMat[6] = "SELECT * FROM je_par WHERE skulture NOT IN ( 111, 112, 113, 114, 115, 116, 117, 118, 121, 122, 123, 124, 125, 126, 127, 128, 131, 132, 133, 134, 135, 136, 137, 138, 141, 142, 143, 144, 145, 146, 147, 148, 151, 152, 153, 154, 155, 156, 157, 158, 161, 162, 163, 164, 165, 166, 167, 168, 171, 172, 173, 174, 175, 176, 177, 178, 181, 182, 183, 184, 185, 186, 187, 188, 207, 213, 208, 209, 220, 212, 210, 205, 216, 206, 308, 309, 360, 313, 252, 290, 321, 324, 317, 310, 322, 323, 307, 306, 303, 304, 319, 318, 320, 305, 325, 370, 380, 202, 203, 214, 215, 249, 251, 253, 299, 302, 314, 316, 350, 201, 204, 211, 301, 311, 312, 315, 217, 218, 219, 361 )";
            sqlMat[7] = "SELECT * FROM je_par where brstavke=0 or brstavke=\"\" or brstavke is null";

            commentarMat[0] = "001 Oznaka za broj i podbroj parcele.................................Par";
            commentarMat[1] = "002 Oznaka za broj B_lista ..........................................Par ";
            commentarMat[2] = "004 Oznaka za plan, skicu/god. ili manual/god........................Par ";
            commentarMat[3] = "006 Oznaka i {ifra ulice ili potesa..................................Par ";
            commentarMat[4] = "008  Oznaka za povr{inu parcele (hektari,ari,metri)...................Par";
            commentarMat[5] = "010 Oznaka za gra|evinsko zemqi{te...................................Par ";
            commentarMat[6] = "011 [ifra na~ina kori{}ewa zemqi{ta (kultura)........................Par ";
            commentarMat[7] = "013 Oznaka za broj stavke na parceli.................................Par ";

            // KONTROLA KOR BAZE

            sqlMat[8] = "SELECT * from je_kor where brposlis=\"\" or brposlis is null or brposlis=0 or cast(brposlis as UNSIGNED)=0";
            sqlMat[9] = "SELECT * from je_kor where prezime is null or prezime=\"\"";
            sqlMat[10] = "SELECT * from je_kor where mesto is null or mesto=\"\"";
            sqlMat[11] = "SELECT * FROM je_kor WHERE broj REGEXP '[A-Za-z/_)(*&^%$#@]'";
            sqlMat[12] = "SELECT * FROM je_kor where cast(matbrgra as UNSIGNED)=0 or matbrgra=\"\" or matbrgra is null or LENGTH(matbrgra)>13";

            sqlMat[13] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)>cast(imenilac as UNSIGNED)) and (obimprava<>4 and obimprava<>1)";
            sqlMat[14] = "SELECT * FROM je_kor where obimprava<>4 and brojilac=\"0000Z\" and imenilac=\"0000S\"";
            sqlMat[15] = "SELECT * FROM je_kor where obimprava=1 and (brojilac<>\"000001\" or imenilac<>\"000001\")";

            sqlMat[16] = "SELECT * FROM JE_kor WHERE ds_ps NOT IN (SELECT sifra FROM kat_svojina)";
            sqlMat[17] = "SELECT brposlis, matbrgra, prezime, ds_ps,vrstaprava FROM je_kor WHERE vrstaprava=5 and ds_ps<>2 UNION SELECT brposlis, matbrgra, prezime, ds_ps,vrstaprava FROM je_kor WHERE VRSTAPRAVA not in (select sifra from kat_pravovrsta)";
            //sqlMat[18] = "SELECT brposlis, matbrgra, prezime, ds_ps,vrstaprava FROM je_kor WHERE vrstaprava=5 and ds_ps<>2";
            sqlMat[19] = "SELECT * FROM je_kor WHERE obimprava NOT IN ( SELECT sifra FROM kat_pravoobim )";
            sqlMat[20] = "select * from (SELECT * FROM je_kor LEFT OUTER JOIN kat_nosiocip on je_kor.sifralica=kat_nosiocip.SIFRA ) A WHERE naziv is null";
            sqlMat[21] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)/cast(imenilac as UNSIGNED))=1 AND obimprava<>1";
            sqlMat[22] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)/cast(imenilac as UNSIGNED))<>1 AND obimprava>2";
            sqlMat[23] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)/cast(imenilac as UNSIGNED))=1 AND vrstaprava<>1";
            sqlMat[24] = "SELECT * FROM je_kor where sifralica in (2000,2001) AND (obimprava>2 or ds_ps>1) and BROJILAC<>'00000Z'";

            commentarMat[8] = "015 Oznaka za broj B_lista...........................................Kor ";
            commentarMat[9] = "017  Oznaka za prezime ili naziv pravnog lica.........................Kor";
            commentarMat[10] = "018  Oznaka za mesto stanovawa ili sedi{te pravnog lica...............Kor ";
            commentarMat[11] = "019  Oznaka za ku}ni broj i podbroj...................................Kor ";
            commentarMat[12] = "020 Oznaka za mati~ni broj gra|ana ili pravnog lica..................Kor ";

            commentarMat[13] = "021  Oznaka za udeo (brojilac/imenilac)...............................Kor ";
            commentarMat[14] = "021  Oznaka za udeo (brojilac/imenilac)...............................Kor ";
            commentarMat[15] = "021  Oznaka za udeo (brojilac/imenilac)...............................Kor ";

            commentarMat[16] = "022 Oznaka za vrstu svojine..........................................Kor ";
            commentarMat[17] = "024 Oznaka za vrstu prava............................................Kor ";
            //commentarMat[18] = "024 Oznaka za vrstu prava............................................Kor ";
            commentarMat[19] = "025 Oznaka za obim prava.............................................Kor ";
            commentarMat[20] = "026  Oznaka za {ifru lica.............................................Kor ";

            commentarMat[21] = "026  Obim prava.............................................Kor ";
            commentarMat[22] = "026  Obim prava.............................................Kor ";
            commentarMat[23] = "026  Vrsta prava.............................................Kor ";
            commentarMat[24] = "026  DS-PS.............................................Kor ";


            // KONTROLA KOR BAZE
            sqlMat[25] = "select * FROM je_vli where cast(brojparc as UNSIGNED)=0 or (cast(podbroj as UNSIGNED)=0 and podbroj<>\"000\") or brojparc is null or podbroj is null or length(brojparc)<>5 or length(podbroj)<>3";
            sqlMat[26] = "SELECT 'obj', brojparc, podbroj, zk_br, nacinkor, evidencija FROM je_vli WHERE EVIDENCIJA <> 0 AND NACINKOR NOT IN (3001, 3002, 3003) UNION SELECT 'deo', brojparc, podbroj, zk_br, nacinkor, evidencija FROM je_vli WHERE EVIDENCIJA = 0 AND NACINKOR IN (3001, 3002, 3003)";
            sqlMat[27] = "select * FROM je_vli where zk_br=0 or zk_br is null";

            sqlMat[28] = "select * FROM je_vli where pstatuso=9 and nacinkor not in (\"10040\",\"10050\",\"10049\",\"10059\",\"20103\") ";
            sqlMat[29] = "SELECT * FROM je_vli LEFT OUTER JOIN kat_objektiosnovizg on je_vli.pstatuso = kat_objektiosnovizg.SIFRA where OSNOVIZG is null";
            sqlMat[30] = "SELECT * FROM je_vli WHERE pstatuso<>0 and nacinkor in (\"3001\",\"3002\", \"3003\")";

            //SELECT * FROM je_vli WHERE NACINKOR IN (3001, 3002, 3003) AND EVIDENCIJA = 0 UNION SELECT * FROM je_vli WHERE NACINKOR NOT IN (3001, 3002, 3003) AND EVIDENCIJA <> 0
            
            sqlMat[31] = "SELECT BROJPARC,STRUKTURA,NACINKOR FROM je_vli WHERE (NACINKOR NOT IN(3001, 3002, 3003)) AND((STRUKTURA <> '') AND (STRUKTURA IS NOT NULL))";
            sqlMat[32] = "SELECT * FROM je_vli WHERE povrsina=0 or povrsina is null AND (povrsina>0 and nacinpovr=7)";
            sqlMat[33] = "SELECT BROJPARC,NACINKOR,NACINPOVR FROM je_vli WHERE (NACINKOR not IN (3001, 3002, 3003)) AND NACINPOVR<>0 ORDER BY BROJPARC";
            sqlMat[34] = "select brojparc, brojilac, imenilac, OBIMPRAVA FROM je_vli WHERE instr(BROJILAC,'Z')<>0 and OBIMPRAVA<>4 UNION select brojparc, brojilac, imenilac, OBIMPRAVA FROM je_vli WHERE BROJILAC='000001' AND IMENILAC='000001' and OBIMPRAVA<>1 UNION select brojparc, brojilac, imenilac, OBIMPRAVA FROM je_vli WHERE IMENILAC<>'000001' and OBIMPRAVA=1 UNION select brojparc, brojilac, imenilac, OBIMPRAVA FROM je_vli WHERE BROJILAC>IMENILAC and instr(BROJILAC,'Z')=0 UNION select brojparc, brojilac, imenilac, OBIMPRAVA FROM je_vli	WHERE instr(BROJILAC,'Z')<>0 and OBIMPRAVA <> 4";
            sqlMat[35] = "SELECT brojparc, podbroj, zk_br FROM je_vli where (svojina1 is null) or (svojina1 not in (select sifra FROM kat_svojina))";
            sqlMat[36] = "SELECT * FROM je_vli where sifralica is null or sifralica=\"\"";
            sqlMat[37] = "SELECT * FROM je_vli where matbrgra is null or matbrgra=\"\"";
            sqlMat[38] = "SELECT * FROM je_vli where matbrgra is null or matbrgra=\"\"";
            sqlMat[39] = "SELECT * FROM je_vli WHERE NACINKOR <> 3002 AND zku <> 0 UNION SELECT * FROM je_vli WHERE NACINKOR = 3002 AND ZKU = 0";
            
            commentarMat[25] = "028 Oznaka za broj i podbroj parcele.................................Vli ";
            commentarMat[26] = "029 Oznaka za objekat ili poseban deo objekta........................Vli ";
            commentarMat[27] = "030 Oznaka za redni broj objekta.....................................Vli ";

            commentarMat[28] = "031 Oznaka za pravni status objekta..................................Vli ";
            commentarMat[29] = "031 Oznaka za pravni status objekta..................................Vli ";
            commentarMat[30] = "031 Oznaka za pravni status objekta..................................Vli ";
            
            commentarMat[31] = "037 Povr{ina objekta ili posebnog dela objekta ......................Vli ";
            commentarMat[32] = "034 Povr{ina objekta ili posebnog dela objekta ......................Vli ";
            commentarMat[33] = "038 Oznaka za na~in utvr|ivawa korisne povr{ine poseb.dela objekta...Vli ";
            commentarMat[34] = "042 Oznaka za udeo (brojilac/imenilac)...............................Vli ";
            commentarMat[35] = "039 Oznaka za vrstu svojine..........................................Vli ";
            commentarMat[36] = "043 Oznaka za {ifru lica.............................................Vli ";
            commentarMat[37] = "044 Oznaka za mati~ni broj gra|ana ili pravnog lica..................Vli ";
            commentarMat[38] = "045 Oznaka za prezime fizi~kog ili naziv pravnog lica................Vli ";
            commentarMat[39] = "047 Na~in kori{}ewa poslovnog prostora posebnog dela objekta.........Vli ";

            // KONTROLA UPARENOSTI BAZA
            sqlMat[40] = "select brojparc,podbroj,count(*) from (select DISTINCT brojparc,podbroj,brposlis,brojplana,sta_krug FROM je_par ) as AA GROUP BY brojparc,podbroj having count(*)>1";
            sqlMat[41] = "SELECT sulice, broj, uzbroj,count(*) FROM je_par where (broj is not null and uzbroj is not null) AND (broj <>'' and uzbroj<>'') GROUP BY sulice, broj, uzbroj HAVING count(*)>1";
            sqlMat[42] = "select brojparc,podbroj,skulture,count(*) FROM je_par where skulture not in (370,360,361) GROUP BY brojparc,podbroj,skulture having count(*)>1";
            sqlMat[43] = "SELECT DISTINCT brposlis,\"par\" FROM je_par where brposlis not IN (select DISTINCT brposlis FROM je_kor) union SELECT DISTINCT brposlis,\"kor\" FROM je_kor where brposlis not IN (select DISTINCT brposlis FROM je_par)";
            sqlMat[44] = "SELECT je_par.brojparc, je_par.podbroj, je_par.brstavke, je_par.hektari * 10000 + je_par.ari * 100 + je_par.metri AS PovrsinaPAR, POVRSINA AS povrsinaVLI, skulture FROM je_par LEFT OUTER JOIN je_vli ON je_par.brojparc = je_vli.BROJPARC AND je_par.podbroj = je_vli.PODBROJ AND je_par.brstavke = je_vli.ZK_BR WHERE (skulture = 360 OR skulture = 361) AND ( NACINKOR <> 3001 AND NACINKOR <> 3002 AND NACINKOR <> 3003 ) AND cast( hektari * 10000 + ari * 100 + metri AS UNSIGNED ) <> cast(POVRSINA AS UNSIGNED) UNION SELECT je_par.brojparc, je_par.podbroj, je_par.brstavke, je_par.hektari * 10000 + je_par.ari * 100 + je_par.metri AS PovrsinaPAR, POVRSINA AS povrsinaVLI, skulture FROM je_par LEFT OUTER JOIN je_vli ON je_par.brojparc = je_vli.BROJPARC AND je_par.podbroj = je_vli.PODBROJ AND je_par.brstavke = je_vli.ZK_BR WHERE (skulture = 360 OR skulture = 361) AND ( NACINKOR <> 3001 AND NACINKOR <> 3002 AND NACINKOR <> 3003 ) AND povrsina IS NULL UNION SELECT je_par.brojparc, je_par.podbroj, je_par.brstavke, je_par.hektari * 10000 + je_par.ari * 100 + je_par.metri AS PovrsinaPAR, POVRSINA AS povrsinaVLI, skulture FROM je_par LEFT OUTER JOIN je_vli ON je_par.brojparc = je_vli.BROJPARC AND je_par.podbroj = je_vli.PODBROJ AND je_par.brstavke = je_vli.ZK_BR WHERE (skulture = 360 OR skulture = 361) AND povrsina IS NULL ORDER BY BROJPARC, podbroj";
            sqlMat[45] = "SELECT * FROM (( SELECT * FROM je_par WHERE brposlis IN ( SELECT brposlis FROM je_kor WHERE VRSTAPRAVA = 5 ) OR skulture = 361 ) AA LEFT OUTER JOIN je_gli ON AA.brojparc = je_gli.BROJPARC AND AA.podbroj = je_gli.PODBROJ AND AA.brstavke = je_gli.ZK_BR )";
            //sqlMat[46] = "SELECT * FROM (SELECT A.*,je_gli.SIFRA FROM (SELECT distinct brposlis,brojparc,podbroj FROM je_par where skulture=361) A LEFT OUTER JOIN je_gli on A.brojparc=je_gli.BROJPARC and A.podbroj=je_gli.PODBROJ) B WHERE sifra is null or sifra<>147;";
            //sqlMat[47] = "SELECT * FROM (SELECT DISTINCT A.*,je_par.skulture FROM (SELECT BROJPARC,PODBROJ, ZK_BR FROM je_gli where SIFRA=147) A LEFT OUTER JOIN je_par on A.BROJPARC=je_par.brojparc and A.PODBROJ=je_par.podbroj AND A.ZK_BR=je_par.brstavke) B where skulture is null or skulture=\"\" or skulture<>361";

            //sqlMat[48] = "SELECT * FROM ( SELECT brposlis, SIFRALICA, MATBRGRA, DS_PS, VRSTAPRAVA, OBIMPRAVA, BROJILAC, IMENILAC FROM je_kor ) A INNER JOIN ( SELECT brposlis, SIFRALICA, MATBRGRA, DS_PS, VRSTAPRAVA, OBIMPRAVA, BROJILAC, IMENILAC FROM je_kor ) B ON A.SIFRALICA = B.SIFRALICA AND A.MATBRGRA = B.MATBRGRA AND A.DS_PS = B.DS_PS AND A.VRSTAPRAVA = B.VRSTAPRAVA AND A.OBIMPRAVA = B.OBIMPRAVA AND A.BROJILAC = B.BROJILAC AND A.IMENILAC = B.IMENILAC AND A.brposlis <> B.brposlis";
            sqlMat[48] = "SELECT * FROM (SELECT GROUP_CONCAT(matbrgra, sifralica, brojilac, imenilac) kk, brposlis FROM je_kor GROUP BY brposlis ORDER BY brposlis ) AA GROUP BY kk having count(*)>1";
            sqlMat[49] = "select brposlis, sum(cast(brojilac as UNSIGNED))/cast(imenilac as UNSIGNED) MJ from je_kor GROUP BY brposlis having MJ<>1";
            sqlMat[50] = "SELECT * FROM je_kor WHERE MATBRGRA IN ( SELECT MATBRGRA FROM ( SELECT DISTINCT MATBRGRA, PREZIME, IMEOCA, IME, MESTO, ULICA, BROJ FROM je_kor ) AA GROUP BY MATBRGRA HAVING count(*) > 1 ORDER BY MATBRGRA ) ORDER BY MATBRGRA";
            sqlMat[51] = "SELECT count(*),brposlis from (SELECT DISTINCT brposlis, obimprava FROM je_kor ORDER BY brposlis) A GROUP BY brposlis HAVING count(*)>1";
            sqlMat[52] = "SELECT DISTINCT brposlis, \"nema u kor\" FROM je_par where brposlis not IN (select DISTINCT brposlis FROM je_kor) union SELECT DISTINCT brposlis, \"nema u par\" FROM je_kor where brposlis not IN (select DISTINCT brposlis FROM je_par)";
            sqlMat[53] = "SELECT * FROM ( SELECT * FROM ( SELECT DISTINCT MATBRGRA, SIFRALICA FROM je_vli WHERE MATBRGRA IS NOT NULL ) AA LEFT OUTER JOIN ( SELECT matbrgra mb1, sifralica sl1 FROM je_lica ) BB ON AA.MATBRGRA = BB.mb1 AND AA.SIFRALICA = BB.sl1 ) GG WHERE mb1 IS NULL ORDER BY matbrgra";
            sqlMat[54] = "SELECT brojparc, podbroj FROM je_vli GROUP BY BROJPARC, PODBROJ, ZK_BR, EVIDENCIJA, MATBRGRA having count(*)>1";
            sqlMat[55] = "SELECT BROJPARC, PODBROJ FROM (SELECT distinct brojparc, PODBROJ, OBIMPRAVA FROM je_vli) aa GROUP BY brojparc, podbroj, obimprava having count(*)>1";
            sqlMat[56] = "SELECT BROJPARC,PODBROJ, OBIMPRAVA, IMENILAC, BROJILAC FROM je_vli where IMENILAC='000001' and BROJILAC='000001' and OBIMPRAVA<>1 union SELECT BROJPARC,PODBROJ, OBIMPRAVA, IMENILAC, BROJILAC FROM  je_vli where IMENILAC<>'000001' and BROJILAC<>'000001' and OBIMPRAVA=1";
            sqlMat[57] = "SELECT * FROM ( SELECT brojparc, PODBROJ, ZK_BR, sum(BROJILAC) sboj, cast(IMENILAC AS UNSIGNED) sime FROM je_vli WHERE OBIMPRAVA = 4 GROUP BY brojparc, PODBROJ, ZK_BR ) AA WHERE sboj <> sime";
            sqlMat[58] = "SELECT * from (SELECT * FROM (SELECT DISTINCT brojparc BR,podbroj PB FROM je_gli) A LEFT OUTER JOIN (SELECT distinct BROJPARC as BR1, PODBROJ PB1 FROM je_par ) B on A.BR=B.BR1 and A.PB=B.PB1) C where br1 is null ";
            sqlMat[59] = "SELECT * from (SELECT * FROM (select DISTINCT BROJPARC BR1 ,PODBROJ PB1,ZK_BR ZK1 FROM je_gli where SIFRA=141) A LEFT OUTER JOIN (select DISTINCT brojparc BR2,podbroj PB2,zk_br ZK2 FROM je_vli where pstatuso=5 ) B on A.BR1=B.BR2 and A.PB1=B.PB2 and A.ZK1=B.ZK2) C where Br2 is null;  ";
            sqlMat[60] = "SELECT * from (SELECT * FROM (select DISTINCT BROJPARC BR1 ,PODBROJ PB1,ZK_BR ZK1 FROM je_gli where SIFRA>=141 and SIFRA<=148) A inner JOIN (select DISTINCT brojparc BR2,podbroj PB2,zk_br ZK2 FROM je_vli where pstatuso=9 ) B on A.BR1=B.BR2 and A.PB1=B.PB2 and A.ZK1=B.ZK2) C where Br2 is null ";
            //sqlMat[61] = "select * FROM je_gli where (matbrgra is null or  sifralica is null or matbrgra='' or sifralica='') and sifra not in(138,147,141,111,152,153)";
            sqlMat[61] = "SELECT * FROM ( SELECT * FROM ( SELECT DISTINCT MATBRGRA, SIFRALICA FROM je_gli WHERE MATBRGRA IS NOT NULL ) AA LEFT OUTER JOIN ( SELECT matbrgra mb1, sifralica sl1 FROM je_lica ) BB ON AA.MATBRGRA = BB.mb1 AND AA.SIFRALICA = BB.sl1 ) GG WHERE mb1 IS NULL ORDER BY matbrgra";
            sqlMat[62] = "SELECT count(*),matbrgra FROM je_lica GROUP BY matbrgra having count(*)>1";
            sqlMat[63] = "SELECT matbrgra, prezime, imeoca, ime, mesto, adresa, broj, uzbroj, count(*) FROM je_lica GROUP BY prezime, imeoca, ime, mesto, adresa, broj, uzbroj HAVING count(*) > 1";

            commentarMat[40] = "072 Da li su zajedni~ki podaci na A_listu jednoobrazni...............Par ";
            commentarMat[41] = "076 Da li postoje objekti sa dupliranim ku}nim brojem................Par ";
            commentarMat[42] = "077 Da li postoje parcele sa dupliranom kulturom i klasom............Par ";
            commentarMat[43] = "080 Da li sve parcele imaju odgovaraju}i broj B_lista............Par-Kor ";
            commentarMat[44] = "082 Da li se objekti u A_listu sla`u sa objektima u V_listu......Par-Vli ";

            commentarMat[45] = "083 Da li su parcele pod zakupom i delom zgrade upisane u G_list.Par-Gli ";
            //commentarMat[46] = "083 Da li su parcele pod zakupom i delom zgrade upisane u G_list.Par-Gli ";
            //commentarMat[47] = "083 Da li su parcele pod zakupom i delom zgrade upisane u G_list.Par-Gli ";

            commentarMat[48] = "086 Da li ima dupliranih B_listova...................................Kor";
            commentarMat[49] = "088 Da li su sume udela u B_listovima = 1............................Kor";
            commentarMat[50] = "089 Da li ima dupliranih mati~nih brojeva............................Kor";
            commentarMat[51] = "090 Da li postoje B_listovi sa razli~itim obimom prava...............Kor";
            commentarMat[52] = "092  Da li svi B_listovi imaju odgovaraju}e A_listove.............Kor-Par";
            commentarMat[53] = "094  Da li su svi korisnici iz B_lista uneti u datoteku Lica.....Kor-Lica";
            commentarMat[54] = "095  Da li su zajedni~ki podaci na V2_listu jednoobrazni..............Vli";
            commentarMat[55] = "099  Da li postoje V listovi sa različitim obimom prava po delovima..............Vli";
            commentarMat[56] = "100  Da li su idealni i realni udeli na v listu korektno upisani..............Vli";
            commentarMat[57] = "101  Da li su sume udela u v listu = 1 ..............Vli";
            commentarMat[58] = "109 Da li svi G_listovi imaju odgovaraju}e A_listove.............Gli-Par";
            commentarMat[59] = "111  Da li tereti na objektima odgovaraju pravnom statusu.........Gli-Vl";
            commentarMat[60] = "111  Da li tereti na objektima odgovaraju pravnom statusu.........Gli-Vl";
            commentarMat[61] = "112  Da li su nosioci tereta iz G_lista uneti u datoteku Lica....Gli-Lica";
            commentarMat[62] = "117 Da li ima dupliranih mati~nih brojeva...........................Lica";
            commentarMat[63] = "118 Da li ima dupliranih indikacija.................................Lica";

            //ono sto smo propustili
            sqlMat[64] = "SELECT BROJPARC, PODBROJ, ZK_BR FROM je_vli where NACINKOR in (3001,3002,3003) AND (ETAZA1<>0 or ETAZA2<>0 or ETAZA3<>0 or ETAZA4<>0)";
            sqlMat[65] = "SELECT * FROM je_vli where BROJSTANA<>0 and EVIDENCIJA=0 ORDER BY BROJPARC, PODBROJ";
            sqlMat[66] = "SELECT brojparc, podbroj, brstavke FROM je_par GROUP by brojparc, podbroj, brstavke having count(*)>1";

            commentarMat[64] = "032 Oznaka za broj eta`a objekta.....................................Vli";
            commentarMat[65] = "036 Oznaka za spratnost posebnog dela objekta .......................Vli";
            commentarMat[66] = "078 Da li su brojevi stavki na parcelama u propisanom redosledu......Par";

            sqlMat[67] = "SELECT DISTINCT MATBRGRA, SIFRALICA, PREZIME, IME, IMEOCA, MESTO, ADRESA, BROJ, UZBROJ FROM je_vli WHERE MATBRGRA IN ( SELECT MATBRGRA FROM ( SELECT DISTINCT MATBRGRA, SIFRALICA, PREZIME, IME, IMEOCA, MESTO, ADRESA, BROJ, UZBROJ FROM je_vli ) A GROUP BY sifralica, matbrgra HAVING count(*) > 1 ) ORDER BY MATBRGRA";
            commentarMat[67] = "098 Da li ima dupliranih mati~nih brojeva............................Vli";

            sqlMat[68] = "SELECT DISTINCT brojparc, podbroj, zk_br, evidencija, Nacinkor, Zku, Nacinpovr, Sprat, Struktura, Brojstana, Slovo, COUNT(*) FROM ( SELECT DISTINCT brojparc, podbroj, zk_br, evidencija, Nacinkor, Zku, Nacinpovr, Sprat, Struktura, Brojstana, Slovo FROM je_vli ) AA GROUP BY brojparc, podbroj, zk_br, evidencija HAVING count(*) > 1";
            commentarMat[68] = "096 Da li su zajedni~ki podaci na V2_listu jednoobrazni..............Vli";

            sqlMat[69] = "SELECT * FROM ( SELECT AA.*, je_vli.POVRSINA AS izlaz, 'pov' FROM ( SELECT brojparc, podbroj, brstavke, ( hektari * 10000 + ari * 100 + metri ) P FROM je_par WHERE skulture = 360 OR skulture = 361 ) AA LEFT OUTER JOIN je_vli ON AA.brojparc = je_vli.BROJPARC AND AA.podbroj = je_vli.PODBROJ AND AA.brstavke = je_vli.ZK_BR AND NACINKOR NOT IN (3001, 3002, 3003)) PP WHERE P <> izlaz UNION SELECT * FROM ( SELECT AA.*, je_vli.EVIDENCIJA AS izlaz, 'evidencija' FROM ( SELECT brojparc, podbroj, brstavke, ( hektari * 10000 + ari * 100 + metri ) P FROM je_par WHERE skulture = 360 OR skulture = 361 ) AA LEFT OUTER JOIN je_vli ON AA.brojparc = je_vli.BROJPARC AND AA.podbroj = je_vli.PODBROJ AND AA.brstavke = je_vli.ZK_BR AND NACINKOR NOT IN (3001, 3002, 3003)) PP WHERE izlaz <> 0 UNION SELECT * FROM ( SELECT AA.*, je_vli.PSTATUSO AS izlaz, 'pstatus' FROM ( SELECT brojparc, podbroj, brstavke, ( hektari * 10000 + ari * 100 + metri ) P FROM je_par WHERE skulture = 361 ) AA LEFT OUTER JOIN je_vli ON AA.brojparc = je_vli.BROJPARC AND AA.podbroj = je_vli.PODBROJ AND AA.brstavke = je_vli.ZK_BR AND NACINKOR NOT IN (3001, 3002, 3003)) PP WHERE izlaz = 7 OR izlaz = 8";
            commentarMat[69] = "105 Da li se objekti u V_listu sla`u sa objektima iz A_listova...Vli-Par";

            sqlMat[70] = "SELECT * FROM ( SELECT DISTINCT AA.*, BB.brojparc prc2 FROM ( SELECT BROJPARC, PODBROJ, ZK_BR FROM je_vli WHERE PSTATUSO = 5 AND EVIDENCIJA = 0 ) AA LEFT OUTER JOIN ( SELECT brojparc, podbroj, zk_br FROM je_gli WHERE sifra = 141 ) BB ON AA.BROJPARC = BB.brojparc AND AA.PODBROJ = BB.podbroj AND AA.ZK_BR = BB.zk_br ORDER BY AA.BROJPARC ) HHJ WHERE prc2 IS NULL";
            commentarMat[70] = "106 Da li objekti 'bez dozvole' u V_listu postoje u G_listu......Vli-Gli";

            sqlMat[71] = "SELECT brojparc, podbroj, zk_br, brp2, 'nacinkor' FROM ( SELECT AA.*, BB.brp2 FROM ( SELECT DISTINCT BROJPARC, PODBROJ, ZK_BR FROM je_vli WHERE NACINKOR IN (3001, 3002, 3003)) AA LEFT OUTER JOIN ( SELECT DISTINCT brojparc brp2, podbroj, zk_br FROM je_vli WHERE NOT NACINKOR IN (3001, 3002, 3003)) BB ON AA.BROJPARC = BB.brp2 AND BB.podbroj = BB.podbroj AND AA.ZK_BR = BB.zk_br ) GG WHERE brp2 IS NULL UNION SELECT brojparc, podbroj, zk_br, brp2, 'evidencija' FROM ( SELECT AA.*, BB.brp2 FROM ( SELECT DISTINCT BROJPARC, PODBROJ, ZK_BR FROM je_vli WHERE evidencija <> 0 ) AA LEFT OUTER JOIN ( SELECT DISTINCT brojparc brp2, podbroj, zk_br FROM je_vli WHERE EVIDENCIJA = 0 ) BB ON AA.BROJPARC = BB.brp2 AND BB.podbroj = BB.podbroj AND AA.ZK_BR = BB.zk_br ) GG WHERE brp2 IS NULL";
            commentarMat[71] = "103 Da li svi posebni delovi objekta imaju odgovaraju}e objekte......Vli";

            sqlMat[72] = "SELECT * FROM ( SELECT * FROM ( SELECT DISTINCT BROJPARC, PODBROJ, ZK_BR FROM je_gli WHERE ZK_BR <> 0 ) AA LEFT OUTER JOIN ( SELECT DISTINCT BROJPARC BRPV, PODBROJ BRPBV, ZK_BR ZKV FROM je_VLI ) BB ON AA.BROJPARC = BB.BRPV AND AA.PODBROJ = BB.BRPBV AND AA.ZK_BR = BB.ZKV ) gg WHERE BRPV IS NULL";
            commentarMat[72] = "110 Da li tereti na objektu imaju odgovaraju}i objekat u V_listu.Gli-Vli";

            //sqlMat[73]="select * from 
            
              

            String[] izlazMat = new String[2];

            izlazMat[0] = sqlMat[gde_];
            izlazMat[1] = commentarMat[gde_];

            return izlazMat;


        }

        private void translate_cirilica_latinica(String nazivtabele)
        {
            //za svako polje u tabeli!
            MySqlConnection con_ = new MySqlConnection(connString_);

            try
            {
                con_.Open();
            }
            catch
            {
                MessageBox.Show("Problem sa konkcijom na bazu");
                return;
            }

            //prvo pretvoris sve u velika slova pa onda ides konverziju slova
            MySqlCommand com_ = new MySqlCommand("select * FROM " + nazivtabele + " limit 1", con_);
            
            MySqlDataReader reader_ = com_.ExecuteReader(CommandBehavior.CloseConnection);

            Int32 i;
            Int32 brojac_ = 0;
            String query_ = "";

            String[] queryij_ = new String[10000];


            for (i = 0; i <= reader_.FieldCount - 1; i++)
            {
                
                string nazivPolja = reader_.GetName(i);
                //sada mozes da napravis queri za jedno po jedno polje i da to ide!
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =ucase(" + nazivPolja + ");";
                brojac_ += 1;

                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Љ','Q');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Њ','W');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'А','A');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Б','B');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'В','V');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Г','G');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Д','D');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Е','E');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'З','Z');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'И','I');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ј','J');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'К','K');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Л','L');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'М','M');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Н','N');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'О','O');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'П','P');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Р','R');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'С','S');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Т','Т');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'У','U');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'ф','F');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ф','F');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Х','H');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ц','C');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ш','[');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ћ',']');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ч','^');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ж','@');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ђ','\');"; brojac_ += 1;
            }
            reader_.Close();
            try
            {
                con_.Open();
            }
            catch { }

            for (i = 0; i <= 1000; i++)
            {
                try
                {
                    if (queryij_[i] != null)
                    {
                        com_.CommandText = queryij_[i];
                        com_.ExecuteNonQuery();
                    }
                    else { break; }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("problem sa " + query_ + "  " + ex.Message);
                }
            }
            MessageBox.Show("Kraj");
        }

        private void translate_latinica_kuke_cirilica(String nazivtabele)
        {
            //za svako polje u tabeli!
            MySqlConnection con_ = new MySqlConnection(connString_);

            try
            {
                con_.Open();
            }
            catch
            {
                MessageBox.Show("Problem sa konkcijom na bazu");
                return;
            }

            //prvo pretvoris sve u velika slova pa onda ides konverziju slova
            MySqlCommand com_ = new MySqlCommand("select * FROM " + nazivtabele + " limit 1", con_);

            MySqlDataReader reader_ = com_.ExecuteReader(CommandBehavior.CloseConnection);

            Int32 i;
            Int32 brojac_ = 0;
            String query_ = "";

            String[] queryij_ = new String[100000];


            for (i = 0; i <= reader_.FieldCount - 1; i++)
            {

                string nazivPolja = reader_.GetName(i);
                //sada mozes da napravis queri za jedno po jedno polje i da to ide!
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =ucase(" + nazivPolja + ");";     brojac_ += 1;

                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Q','Љ');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'W','Њ');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'A','А');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'B','Б');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'V','В');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'G','Г');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'D','Д');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'E','Е');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Z','З');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'I','И');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'J','Ј');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'K','К');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'L','Л');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'M','М');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'N','Н');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'O','О');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'P','П');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'R','Р');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'S','С');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Т','Т');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'U','У');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'F','ф');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'H','Х');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'C','Ц');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'[','Ш');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",']','Ћ');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'^','Ч');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'@','Ж');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'\','Ђ');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'/','Ђ');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Š','Ш');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ć','Ћ');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Č','Ч');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ž','Ж');"; brojac_ += 1;
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Đ','Ђ');"; brojac_ += 1;
            }
            reader_.Close();
            try
            {
                con_.Open();
            }
            catch { }

            for (i = 0; i <= 1000; i++)
            {
                try
                {
                    if (queryij_[i] != null)
                    {
                        com_.CommandText = queryij_[i];
                        com_.ExecuteNonQuery();
                    }
                    else { break; }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("problem sa " + query_ + "  " + ex.Message);
                }
            }
            MessageBox.Show("Kraj");
        }

        private void translate_latinica_latinica_SlovaSimbol(String nazivtabele)
        {
            //za svako polje u tabeli!
            MySqlConnection con_ = new MySqlConnection(connString_);
            
            try
            {
                con_.Open();
            }
            catch
            {
                MessageBox.Show("Problem sa konkcijom na bazu");
                return;
            }

            MySqlCommand com_ = new MySqlCommand("select * FROM " + nazivtabele + " limit 1", con_);
            MySqlCommand comQW_ = new MySqlCommand("", con_);
            MySqlDataReader reader_ = com_.ExecuteReader(CommandBehavior.CloseConnection);

            Int32 i ;
            Int32 brojac_ = 0;
            String query_ = "";

            String[] queryij_ = new String [1000];


            for (i=0; i<=reader_.FieldCount-1; i++)
            {
                string nazivPolja = reader_.GetName(i);
                //sada mozes da napravis queri za jedno po jedno polje i da to ide!
                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Š','[');";
                brojac_ += 1;

                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ž','@');";
                brojac_ += 1;

                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Đ','\');";
                brojac_ += 1;

                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Ć',']')";
                brojac_ += 1;

                queryij_[brojac_] = "update " + nazivtabele + " set " + nazivPolja + " =replace(" + nazivPolja + ",'Č','^')";
                brojac_ += 1;
            }
            reader_.Close();
            try
            {
                con_.Open();
            } catch { }

            for (i=0; i<=1000; i++)
            {
                try
                {
                    if(queryij_[i] != null) { 
                        com_.CommandText = queryij_[i];
                        com_.ExecuteNonQuery();
                    } else { break; }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("problem sa " + query_ + "  " + ex.Message);
                }
            }

            MessageBox.Show("Kraj");

        }

        private void originalLicaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            translate_latinica_latinica_SlovaSimbol("original_lica");

        }

        private void jelicaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            translate_latinica_latinica_SlovaSimbol("je_lica");
        }

        private void jevliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            translate_latinica_latinica_SlovaSimbol("je_vli");
        }

        private void jegliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            translate_latinica_latinica_SlovaSimbol("je_gli");
        }

        private void jekorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            translate_latinica_latinica_SlovaSimbol("je_kor");
        }

        private void jelicaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            translate_cirilica_latinica("je_lica");
        }

        private void jekorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            translate_cirilica_latinica("je_kor");
        }

        private void jevliToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            translate_cirilica_latinica("je_vli");
        }

        private void jegliToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            translate_cirilica_latinica("je_gli");
        }

        private void latinicaCirilicaŠToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //mozda je pametnije da se upise naziv tabele ' jer moye da desi da je u pitanju neka tabela u bazi

        }

        private void nazivTabeleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            translate_cirilica_latinica(txt_nazivTabele.Text);
        }

        private void nazivTabeleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            translate_latinica_latinica_SlovaSimbol(txt_nazivTabele.Text);
        }

        private void latinicaCirilicaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            translate_latinica_kuke_cirilica(txt_nazivTabele.Text);
        }

        private void sirinaPoljaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySqlConnection con_ = new MySqlConnection(connString_);

            try
            {
                con_.Open();
            }
            catch
            {
                MessageBox.Show("Problem sa konkcijom na bazu");
                return;
            }

            //par
            MySqlCommand com_ = new MySqlCommand("alter table je_par MODIFY matbroj VARCHAR(6),	MODIFY brojparc VARCHAR(5),	MODIFY podbroj VARCHAR(3),	MODIFY brstavke VARCHAR(3),	MODIFY BROJPLANA VARCHAR(3),	MODIFY SKICA VARCHAR(3),	MODIFY MANUAL VARCHAR(5),	MODIFY GOD VARCHAR(4),	MODIFY SKULTURE VARCHAR(5),	MODIFY SULICE VARCHAR(5),	MODIFY BROJ VARCHAR(3),	MODIFY UZBROJ VARCHAR(1),	MODIFY BRPOSLIS VARCHAR(5),	MODIFY STA_KRUG VARCHAR(7),	MODIFY GGRADJZEM VARCHAR(1),	MODIFY BONITET VARCHAR(2),	MODIFY HEKTARI VARCHAR(5),	MODIFY ARI VARCHAR(2),	MODIFY METRI VARCHAR(2),	MODIFY ULICAPOTES VARCHAR(1)", con_);
            com_.ExecuteNonQuery();
            //kor
            com_ = new MySqlCommand("alter table je_kor  		MODIFY matbroj VARCHAR(6),		MODIFY BRPOSLIS VARCHAR(5),		MODIFY SIFRALICA VARCHAR(4),		MODIFY MATBRGRA VARCHAR(13),		MODIFY PREZIME VARCHAR(15),		MODIFY IMEOCA VARCHAR(10),		MODIFY IME VARCHAR(10),		MODIFY DEVPREZ VARCHAR(15),		MODIFY IMEMUZA VARCHAR(10),		MODIFY MESTO VARCHAR(12),		MODIFY ULICA VARCHAR(20),		MODIFY BROJ VARCHAR(3),		MODIFY UZBROJ VARCHAR(1),		MODIFY DS_PS VARCHAR(1),		MODIFY VRSTAPRAVA VARCHAR(1),		MODIFY IMENILAC VARCHAR(6),		MODIFY OBVEZNIK VARCHAR(11),		MODIFY JAVNA VARCHAR(1),		MODIFY OBIMPRAVA VARCHAR(1),		MODIFY BROJILAC VARCHAR(6)", con_);
            com_.ExecuteNonQuery();
            //gli
            com_ = new MySqlCommand("alter table je_lica 		MODIFY SIFRALICA VARCHAR(4),		MODIFY MATBRGRA VARCHAR(13),		MODIFY PREZIME VARCHAR(40),		MODIFY IMEOCA VARCHAR(15),		MODIFY IME VARCHAR(15),		MODIFY MESTO VARCHAR(15),		MODIFY ADRESA VARCHAR(30),		MODIFY BROJ VARCHAR(3),		MODIFY UZBROJ VARCHAR(1)", con_);
            com_.ExecuteNonQuery();
            // vli
            com_ = new MySqlCommand("alter table je_vli 		MODIFY MATBROJ VARCHAR(6),		MODIFY BROJPARC VARCHAR(5),		MODIFY PODBROJ VARCHAR(3),		MODIFY ZK_BR VARCHAR(3),		MODIFY PSTATUSO VARCHAR(1),		MODIFY ETAZA1 VARCHAR(4),		MODIFY ETAZA2 VARCHAR(4),		MODIFY ETAZA3 VARCHAR(4),		MODIFY ETAZA4 VARCHAR(4),		MODIFY SLOVO VARCHAR(2),		MODIFY STRUKTURA VARCHAR(3),		MODIFY POVRSINA VARCHAR(6),		MODIFY SIFRALICA VARCHAR(4),		MODIFY MATBRGRA VARCHAR(13),		MODIFY PREZIME VARCHAR(40),		MODIFY IMEOCA VARCHAR(15),		MODIFY IME VARCHAR(15),		MODIFY MESTO VARCHAR(15),		MODIFY ADRESA VARCHAR(30),		MODIFY BROJ VARCHAR(3),		MODIFY UZBROJ VARCHAR(1),		MODIFY BROJILAC VARCHAR(6),		MODIFY IMENILAC VARCHAR(6),		MODIFY EVIDENCIJA VARCHAR(3),		MODIFY NACINKOR VARCHAR(5),		MODIFY PRIMEDBA VARCHAR(50),		MODIFY ZKU VARCHAR(5),		MODIFY NACINPOVR VARCHAR(1),		MODIFY ULAZ VARCHAR(5),		MODIFY SPRAT VARCHAR(4),		MODIFY BROJSTANA VARCHAR(3),		MODIFY SVOJINA1 VARCHAR(1),		MODIFY VRSTAPRAVA VARCHAR(1),		MODIFY OBIMPRAVA VARCHAR(1)", con_);
            com_.ExecuteNonQuery();
            //lica
            com_ = new MySqlCommand("alter table je_gli		MODIFY MATBROJ VARCHAR(6),		MODIFY BROJPARC VARCHAR(5),		MODIFY PODBROJ VARCHAR(3),		MODIFY ZK_BR VARCHAR(3),		MODIFY EVIDENCIJA VARCHAR(3),		MODIFY ZKU VARCHAR(5),		MODIFY ULAZ VARCHAR(5),		MODIFY SPRAT VARCHAR(4),		MODIFY BROJSTANA VARCHAR(3),		MODIFY SLOVO VARCHAR(2),		MODIFY SIFRA VARCHAR(3),		MODIFY CAS VARCHAR(8),		MODIFY DATUM VARCHAR(10),		MODIFY TRAJANJE VARCHAR(10),		MODIFY BRISANJE VARCHAR(10),		MODIFY SIFRALICA VARCHAR(4),		MODIFY MATBRGRA VARCHAR(13)", con_);
            com_.ExecuteNonQuery();

            com_ = new MySqlCommand("UPDATE je_gli set  VRSTATER2 = substring(VRSTATER1,251,250), VRSTATER3 = SUBSTRING(VRSTATER1,501,250), VRSTATER4 = SUBSTRING(VRSTATER1,751,250), VRSTATER5  =SUBSTRING(VRSTATER1,1001,250), VRSTATER1 = SUBSTRING(VRSTATER1,1,250);", con_);
            com_.ExecuteNonQuery();

            com_ = new MySqlCommand("alter table je_gli 	MODIFY VRSTATER1 VARCHAR(250),		MODIFY VRSTATER2 VARCHAR(250),		MODIFY VRSTATER3 VARCHAR(250),		MODIFY VRSTATER4 VARCHAR(250),		MODIFY VRSTATER5 VARCHAR(250);", con_);
            com_.ExecuteNonQuery();

            com_ = null;
            con_.Close();
            con_ = null;

        }

        private void srediPoljaUTabelamaZaPredajuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            of_diag.FileName = "";
            of_diag.Filter = "Access database (*.mdb)|*.mdb | Access database 2000 (*.accdb) |*.accdb";
            of_diag.ShowDialog();

            if (of_diag.FileName == "") return;

            //sada idemo na dao conneciju

          

            String[] sql_ = new String[110];

            sql_[0] = "alter table je_par alter column matbroj text (6)";
            sql_[1] = "alter table je_par alter column brojparc text(5)";
            sql_[2] = "alter table je_par alter column podbroj text(3)";
            sql_[3] = "alter table je_par alter column brstavke text(3)";
            sql_[4] = "alter table je_par alter column brojplana text(3)";
            sql_[5] = "alter table je_par alter column skica text(3)";
            sql_[6] = "alter table je_par alter column manual text(5)";
            sql_[7] = "alter table je_par alter column god text(4)";
            sql_[8] = "alter table je_par alter column skulture text(5)";
            sql_[9] = "alter table je_par alter column hektari text(5)";
            sql_[10] = "alter table je_par alter column ari text(2)";
            sql_[11] = "alter table je_par alter column metri text(2)";
            sql_[12] = "alter table je_par alter column ulicapotes text(1)";
            sql_[13] = "alter table je_par alter column sulice text(5)";
            sql_[14] = "alter table je_par alter column broj text(3)";
            sql_[15] = "alter table je_par alter column uzbroj text(1)";
            sql_[16] = "alter table je_par alter column brposlis text(5)";
            sql_[17] = "alter table je_par alter column sta_krug text(7)";
            sql_[18] = "alter table je_par alter column ggradjzem text(1)";
            sql_[19] = "alter table je_par alter column bonitet text(2)";

            sql_[20] = "alter table je_vli alter column matbroj text(6)";
            sql_[21] = "alter table je_vli alter column brojparc text(5)";
            sql_[22] = "alter table je_vli alter column podbroj text(3)";
            sql_[23] = "alter table je_vli alter column zk_br text(3)";
            sql_[24] = "alter table je_vli alter column pstatuso text(1)";
            sql_[25] = "alter table je_vli alter column etaza1 text(4)";
            sql_[26] = "alter table je_vli alter column etaza2 text(4)";
            sql_[27] = "alter table je_vli alter column etaza3 text(4)";
            sql_[28] = "alter table je_vli alter column etaza4 text(4)";
            sql_[29] = "alter table je_vli alter column evidencija text(3)";
            sql_[30] = "alter table je_vli alter column nacinkor text(5)";
            sql_[31] = "alter table je_vli alter column primedba text(50)";
            sql_[32] = "alter table je_vli alter column zku text(5)";
            sql_[33] = "alter table je_vli alter column nacinpovr text(1)";
            sql_[34] = "alter table je_vli alter column ulaz text(5)";
            sql_[35] = "alter table je_vli alter column sprat text(4)";
            sql_[36] = "alter table je_vli alter column brojstana text(3)";
            sql_[37] = "alter table je_vli alter column slovo text(2)";
            sql_[38] = "alter table je_vli alter column struktura text(3)";
            sql_[39] = "alter table je_vli alter column povrsina text(6)";
            sql_[40] = "alter table je_vli alter column sifralica text(4)";
            sql_[41] = "alter table je_vli alter column matbrgra text(13)";
            sql_[42] = "alter table je_vli alter column prezime text(40)";
            sql_[43] = "alter table je_vli alter column imeoca text(15)";
            sql_[44] = "alter table je_vli alter column ime text(15)";
            sql_[45] = "alter table je_vli alter column mesto text(15)";
            sql_[46] = "alter table je_vli alter column adresa text(30)";
            sql_[47] = "alter table je_vli alter column broj text(3)";
            sql_[48] = "alter table je_vli alter column uzbroj text(1)";
            sql_[49] = "alter table je_vli alter column svojina1 text(1)";
            sql_[50] = "alter table je_vli alter column vrstaprava text(1)";
            sql_[51] = "alter table je_vli alter column obimprava text(1)";
            sql_[52] = "alter table je_vli alter column brojilac text(6)";
            sql_[53] = "alter table je_vli alter column imenilac text(6)";

            sql_[54] = "alter table je_gli alter column matbroj text(6)";
            sql_[55] = "alter table je_gli alter column brojparc text(5)";
            sql_[56] = "alter table je_gli alter column podbroj text(3)";
            sql_[57] = "alter table je_gli alter column zk_br text(3)";
            sql_[58] = "alter table je_gli alter column evidencija text(3)";
            sql_[59] = "alter table je_gli alter column zku text(5)";
            sql_[60] = "alter table je_gli alter column ulaz text(5)";
            sql_[61] = "alter table je_gli alter column sprat text(4)";
            sql_[62] = "alter table je_gli alter column brojstana text(3)";
            sql_[63] = "alter table je_gli alter column slovo text(2)";
            sql_[64] = "alter table je_gli alter column sifra text(3)";
            sql_[65] = "alter table je_gli alter column cas text(8)";
            sql_[66] = "alter table je_gli alter column datum text(10)";
            sql_[67] = "alter table je_gli alter column trajanje text(10)";
            sql_[68] = "alter table je_gli alter column brisanje text(10)";
            sql_[69] = "alter table je_gli alter column vrstater1 text(250)";
            sql_[70] = "alter table je_gli alter column vrstater2 text(250)";
            sql_[71] = "alter table je_gli alter column vrstater3 text(250)";
            sql_[72] = "alter table je_gli alter column vrstater4 text(250)";
            sql_[73] = "alter table je_gli alter column vrstater5 text(250)";
            sql_[74] = "alter table je_gli alter column sifralica text(4)";
            sql_[75] = "alter table je_gli alter column matbrgra text(13)";
            
            sql_[76] = "alter table je_lica alter column sifralica text(4)";
            sql_[77] = "alter table je_lica alter column matbrgra text(13)";
            sql_[78] = "alter table je_lica alter column prezime text(40)";
            sql_[79] = "alter table je_lica alter column imeoca text(15)";
            sql_[80] = "alter table je_lica alter column ime text(15)";
            sql_[81] = "alter table je_lica alter column mesto text(15)";
            sql_[82] = "alter table je_lica alter column adresa text(30)";
            sql_[83] = "alter table je_lica alter column broj text(3)";
            sql_[84] = "alter table je_lica alter column uzbroj text(1)";

            sql_[85] = "alter table je_kor alter column matbroj text(6)";
            sql_[86] = "alter table je_kor alter column brposlis text(5)";
            sql_[87] = "alter table je_kor alter column sifralica text(4)";
            sql_[88] = "alter table je_kor alter column matbrgra text(13)";
            sql_[89] = "alter table je_kor alter column prezime text(15)";
            sql_[90] = "alter table je_kor alter column imeoca text(10)";
            sql_[91] = "alter table je_kor alter column ime text(10)";
            sql_[92] = "alter table je_kor alter column devprez text(15)";
            sql_[93] = "alter table je_kor alter column imemuza text(10)";
            sql_[94] = "alter table je_kor alter column mesto text(12)";
            sql_[95] = "alter table je_kor alter column ulica text(20)";
            sql_[96] = "alter table je_kor alter column broj text(3)";
            sql_[97] = "alter table je_kor alter column uzbroj text(1)";
            sql_[98] = "alter table je_kor alter column ds_ps text(1)";
            sql_[99] = "alter table je_kor alter column vrstaprava text(1)";
            sql_[100] = "alter table je_kor alter column obimprava text(1)";
            sql_[101] = "alter table je_kor alter column brojilac text(6)";
            sql_[102] = "alter table je_kor alter column imenilac text(6)";
            sql_[103] = "alter table je_kor alter column obveznik text(11)";

            sql_[104] = "alter table je_ulice alter column sulice text(5)";
            sql_[105] = "alter table je_ulice alter column nazivulice text(30)";
            sql_[106] = "alter table je_ulice alter column matbroj text(6)";

            sql_[107] = "alter table je_potesi alter column spotesa text(5)";
            sql_[108] = "alter table je_potesi alter column nazivpotes text(20)";
            sql_[109] = "alter table je_potesi alter column matbroj text(6)";

            String connString_ = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + of_diag.FileName + "; Persist Security Info = True";
            OleDbConnection con_ = new OleDbConnection(connString_);
            con_.Open();
            OleDbCommand comm_ = new OleDbCommand(sql_[0], con_);

            pb1.Minimum = 1;
            pb1.Maximum = sql_.Length+1;
            
            foreach (String naredba_ in sql_)
            {
                try
                {
                    comm_.CommandText = naredba_;
                    comm_.ExecuteNonQuery();
                    pb1.Value = pb1.Value + 1;
                } catch
                {
                    MessageBox.Show("Problem sa " + naredba_);
                }
              
            }

            

            MessageBox.Show("Kraj");

        }

        private void townListToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //ajde sada da procitamo iz file-a

            String jsonString = System.IO.File.ReadAllText(@"E:\towns.json");

            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);

            var town_coun = new List<Tuple<string, string>>();

            //city.towns[0].Value
            int i = 0;

            foreach (var city in result)
            {

                for (i=0; i<city.towns.Count ; i++)
                {
                    town_coun.Add(new Tuple<string, string>(city.county.Value, city.towns[i].Value));
                }              
            }

           

            //sada ovo mozemo da zapisemo nekkao

            System.IO.StreamWriter file_ = new System.IO.StreamWriter(@"d:\spisak_count_town.txt");
            
            for (i=0; i<town_coun.Count; i++)
            {
                file_.WriteLine(town_coun[i].Item1 + "," + town_coun[i].Item2);
            }

            MessageBox.Show("Kraj");
        }



        private void clearNullValueFromFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySqlConnection con_ = new MySqlConnection(connString_);
            MySqlConnection con2_ = new MySqlConnection(connString_);

            try
            {
                con_.Open();
                con2_.Open();
            }
            catch
            {
                MessageBox.Show("Problem sa konkcijom na bazu");
                return;
            }

            String[] nazivtabele = { "je_gli", "je_vli", "je_kor", "je_par", "je_lica" };

            //prvo pretvoris sve u velika slova pa onda ides konverziju slova
            
            Int32 i;

            MySqlCommand com_=new MySqlCommand("",con_);
            MySqlCommand comExe_ = new MySqlCommand("", con2_);

            pb1.Value = 0;
            pb1.Maximum = nazivtabele.Length;

            for (int j = 0; j < nazivtabele.Length; j++)
            {

                try
                {
                    con_.Open();
                } catch (Exception ex)
                {
                }
                    


                com_.CommandText = "select * FROM " + nazivtabele[j] + " limit 1";

                MySqlDataReader reader_ = com_.ExecuteReader(CommandBehavior.CloseConnection);

                
                for (i = 0; i <= reader_.FieldCount - 1; i++)
                {

                    string nazivPolja = reader_.GetName(i);
                    //sada mozes da napravis queri za jedno po jedno polje i da to ide!
                    comExe_.CommandText = "update " + nazivtabele[j] + " set " + nazivPolja + " ='' where " + nazivPolja + " is null;";
                    comExe_.ExecuteNonQuery();

                }
                pb1.Value = j;
                reader_.Close();

            }

            try
            {
                con2_.Close();
            } catch
            {

            }
           
            MessageBox.Show("Kraj");
        }

        private void jSONToXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("pronadi putanju");

        }
    }
}
