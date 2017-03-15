using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Converter.v5_2
{
   public interface IFormulaMapper
   {
      string NewFormulaFor(string oldFormula);
      bool FormulaWasConverted(string newFormula);
   }

   internal class FormulaMapper : IFormulaMapper
   {
      private readonly ICache<string, string> _formulaCache;

      public FormulaMapper()
      {
         _formulaCache = new Cache<string, string> {OnMissingKey = s => string.Empty};
         intitializeMapping();
      }

      private void intitializeMapping()
      {
         map("OralApplicationsEnabled ? P_int_para*Ageom*AeffFactor*(min(Solubility/ MW ;DrugLiquid/(Liquid))-(NOT SinkCondition_para ? fu*DrugMucosa : 0))*1e-3 : 0", "OralApplicationsEnabled ? P_int_para*Ageom*AeffFactor*(min(Solubility/ MW ;DrugLiquid/(Liquid))-(NOT SinkCondition_para ? fu*DrugMucosa : 0)) : 0");
         map("OralApplicationsEnabled ? P_int_trans*Ageom*AeffFactor*(P_int_trans_lum_cell_factor*min(Solubility/ MW ;DrugLiquid/(Liquid))-(NOT SinkCondition_trans ? P_int_trans_cell_lum_factor*fu/K_cell_pls*DrugMucosa : 0))*1e-3 : 0", "OralApplicationsEnabled ? P_int_trans*Ageom*AeffFactor*(P_int_trans_lum_cell_factor*min(Solubility/ MW ;DrugLiquid/(Liquid))-(NOT SinkCondition_trans ? P_int_trans_cell_lum_factor*fu/K_cell_pls*DrugMucosa : 0)) : 0");

         map("Min(CP * f_int * V * kcat  / ( Km*(1 + Inhibitor/Ki) + C_pls * fu) ; P_endothelial * SA /1000 )* C_pls * fu + CP * f_int * V * kcat * K_water_int * C_int / (Km*(1 + Inhibitor/Ki) + K_water_int * C_int)", "Min(CP * f_int * V * kcat  / ( Km*(1 + Inhibitor/Ki) + C_pls * fu) ; P_endothelial * SA)* C_pls * fu + CP * f_int * V * kcat * K_water_int * C_int / (Km*(1 + Inhibitor/Ki) + K_water_int * C_int)");
         map("Min(CP * f_int * V * kcat  / ( Km^alpha + C_pls^alpha * fu) ; P_endothelial * SA /1000 )* C_pls^alpha * fu + CP * f_int * V * kcat * K_water_int * C_int^alpha / (Km^alpha + K_water_int * C_int^alpha)", "Min(CP * f_int * V * kcat  / ( Km^alpha + C_pls^alpha * fu) ; P_endothelial * SA)* C_pls^alpha * fu + CP * f_int * V * kcat * K_water_int * C_int^alpha / (Km^alpha + K_water_int * C_int^alpha)");
         map("Min(CP * f_int * V * kcat  / ( Km + C_pls * fu) ; P_endothelial * SA /1000 )* C_pls * fu + CP * f_int * V * kcat * K_water_int * C_int / (Km + K_water_int * C_int)", "Min(CP * f_int * V * kcat  / ( Km + C_pls * fu) ; P_endothelial * SA)* C_pls * fu + CP * f_int * V * kcat * K_water_int * C_int / (Km + K_water_int * C_int)");
         map("min(TM * kcat / (Km*(1 + Inhibitor/Ki)+min(Solubility/MW; DrugLiquid/Liquid)); P_int_trans*Ageom*AeffFactor*P_int_trans_lum_cell_factor* 1e-3) * min(Solubility/MW; DrugLiquid/Liquid) + TM * kcat * K_water_cell * C_cell / (Km*(1 + Inhibitor/Ki)+K_water_cel", "min(TM * kcat / (Km*(1 + Inhibitor/Ki)+min(Solubility/MW; DrugLiquid/Liquid)); P_int_trans*Ageom*AeffFactor*P_int_trans_lum_cell_factor) * min(Solubility/MW; DrugLiquid/Liquid) + TM * kcat * K_water_cell * C_cell / (Km*(1 + Inhibitor/Ki)+K_water_cel");
         map("min(TM * kcat / (Km^alpha+min(Solubility/MW; DrugLiquid/Liquid)^alpha); P_int_trans*Ageom*AeffFactor*P_int_trans_lum_cell_factor* 1e-3) * min(Solubility/MW; DrugLiquid/Liquid)^alpha + TM * kcat * K_water_cell * C_cell^alpha / (Km^alpha+K_water_cell * C_ce", "min(TM * kcat / (Km^alpha+min(Solubility/MW; DrugLiquid/Liquid)^alpha); P_int_trans*Ageom*AeffFactor*P_int_trans_lum_cell_factor) * min(Solubility/MW; DrugLiquid/Liquid)^alpha + TM * kcat * K_water_cell * C_cell^alpha / (Km^alpha+K_water_cell * C_ce");
         map("min(TM * kcat / (Km+min(Solubility/MW; DrugLiquid/Liquid)); P_int_trans*Ageom*AeffFactor*P_int_trans_lum_cell_factor* 1e-3) * min(Solubility/MW; DrugLiquid/Liquid) + TM * kcat * K_water_cell * C_cell / (Km+K_water_cell * C_cell)", "min(TM * kcat / (Km+min(Solubility/MW; DrugLiquid/Liquid)); P_int_trans*Ageom*AeffFactor*P_int_trans_lum_cell_factor) * min(Solubility/MW; DrugLiquid/Liquid) + TM * kcat * K_water_cell * C_cell / (Km+K_water_cell * C_cell)");
         map("0,0333 * MW ^ 0,4226", "0,0333 * (MW * 1E9) ^ 0,4226 * 1E-8");
         map("Dose / MW * 1000", "Dose / MW");
         map("60 * 10^(-4.113-0.4609*Log10(MW))", "60 * 10^(-4.113-0.4609*Log10(MW * 1E9)) *1E-2");
         map("Height>0 ? BW / (Height / 100)^2 : 0", "Height>0 ? BW / (Height)^2 : 0");
         map("InVitroCL/CYP * 1e-6", "InVitroCL/CYP");
         map("Min(1/((1 - M_Prot * 0,001) + 10^LogMA * M_Prot * 0,001);1)", "Min(1/((1 - M_Prot) + 10^LogMA * M_Prot);1)");
         map("P_endothelial_l*SA_pls_int/1000*Pe_l/(exp(Pe_l)-1) + P_endothelial_s*SA_pls_int/1000*Pe_s/(exp(Pe_s)-1)", "P_endothelial_l*SA_pls_int*Pe_l/(exp(Pe_l)-1) + P_endothelial_s*SA_pls_int*Pe_s/(exp(Pe_s)-1)");
         map("InVitroVmaxPerRecombinantEnzyme * 1000", "InVitroVmaxPerRecombinantEnzyme");
         map("InVitroVmaxPerTransporter * 1000", "InVitroVmaxPerTransporter");
         map("(0.8292 *(1-exp(-Energy/578.4)) +0.13) *exp(Meal_beta*5.17)", "(0.8292 *(1-exp(-Energy/(4184*10*10*60*60)/578.4)) +0.13) *exp(Meal_beta*5.17)");
         map("MW - F * 17 - Cl * 22 - Br * 62 - I * 98", "MW - F * 0.000000017 - Cl * 0.000000022 - Br * 0.000000062 - I * 0.000000098");
         map("DrugMass / DrugDensity * 1E-6 * MW * Number_Of_Particles_Factor", "DrugMass / DrugDensity * MW * Number_Of_Particles_Factor");
         map("IsSmallMolecule ? (MWEff / 336) ^ (-6) * 10^LogMA / 5 * 1E-4 : 0", "IsSmallMolecule ? (MWEff *1E9 / 336) ^ (-6) * 10^LogMA / 5 * 1E-4 *1E-1: 0");
         map("gamma<1 ? (1-gamma)^4.5/(1-0.3956*gamma+1.0616*gamma^2)* alpha_l*8*Lp/(r*1e-7)^2 * RT/(6*Pi*Na*a_e*1e-7) : 1e-22", "gamma<1 ? (1-gamma)^4.5/(1-0.3956*gamma+1.0616*gamma^2)* alpha_l*8*Lp/(r)^2 * RT/(6*Pi*Na*a_e) : 1e-22");
         map("gamma<1 ? (1-gamma)^4.5/(1-0.3956*gamma+1.0616*gamma^2)* alpha_l*8*Lp/(r*1e-7)^2 * RT/(6*Pi*Na*a_e_endog*1e-7) : 1e-22", "gamma<1 ? (1-gamma)^4.5/(1-0.3956*gamma+1.0616*gamma^2)* alpha_l*8*Lp/(r)^2 * RT/(6*Pi*Na*a_e_endog) : 1e-22");
         map("gamma<1 ? (1-gamma)^4.5/(1-0.3956*gamma+1.0616*gamma^2)* (1-alpha_l)*8*Lp/(r*1e-7)^2 * RT/(6*Pi*Na*a_e*1e-7) : 1e-22", "gamma<1 ? (1-gamma)^4.5/(1-0.3956*gamma+1.0616*gamma^2)* (1-alpha_l)*8*Lp/(r)^2 * RT/(6*Pi*Na*a_e) : 1e-22");
         map("gamma<1 ? (1-gamma)^4.5/(1-0.3956*gamma+1.0616*gamma^2)* (1-alpha_l)*8*Lp/(r*1e-7)^2 * RT/(6*Pi*Na*a_e_endog*1e-7) : 1e-22", "gamma<1 ? (1-gamma)^4.5/(1-0.3956*gamma+1.0616*gamma^2)* (1-alpha_l)*8*Lp/(r)^2 * RT/(6*Pi*Na*a_e_endog) : 1e-22");
         map("10 ^ (m * log10 (P_int_InVitro) + b)", "10 ^ (m * log10 (P_int_InVitro*10) + b) * 1E-1");
         map("10 ^ (m * log10 (P_int_InVitro*1e6) + b)*1e-6", "10 ^ (m * log10 (P_int_InVitro*10*1e6) + b)*1e-6 * 1E-1");
         map("IsSmallMolecule ? (266 * (MWEff ^ ( - 4.5) * 10^LogMA))*60 : 0", "IsSmallMolecule ? 266 * (MWEff * 1E9) ^ ( - 4.5) * 10^LogMA * 60 *1E-1 : 0");
         map("P_cell_int * SA /1000", "P_cell_int * SA");
         map("P_int_cell * SA /1000", "P_int_cell * SA");
         map("N * DrugDensity * 4/3 * Pi * (r0*1E-7)^3 * 1E6 / MW", "N * DrugDensity * 4/3 * Pi * (r0)^3 / MW");
         map("PrecipitatedDrugSoluble ? 0 : (X>0 ? 3*D*X0^(1/3)/(rho*1E9/MW)/(h*1E-7)/(r0*1E-7)*X^(2/3)*min(S/MW - DrugLiquid/Liquid; 0) : 0)", "PrecipitatedDrugSoluble ? 0 : (X>0 ? 3*D*X0^(1/3)/(rho/MW)/(h)/(r0)*X^(2/3)*min(S/MW - DrugLiquid/Liquid; 0) : 0)");
         map("X>0 ? 3*D*X0^(1/3)/(rho*1E9/MW)/(h*1E-7)/(r0*1E-7)*X^(2/3)*(PrecipitatedDrugSoluble ? S/MW - DrugLiquid/Liquid : max(S/MW - DrugLiquid/Liquid; 0)) : 0", "X>0 ? 3*D*X0^(1/3)/(rho/MW)/(h)/(r0)*X^(2/3)*(PrecipitatedDrugSoluble ? S/MW - DrugLiquid/Liquid : max(S/MW - DrugLiquid/Liquid; 0)) : 0");

         map("(V_bon * 1000 / 28.2)^0.75 * 10 * 1E4", "(V_bon * 1000 / 28.2)^0.75 * 10 * 1E2");
         map("(V_brn  * 1000 / 1.671)^0.75 * 0.0006 * 1E4", "(V_brn  * 1000 / 1.671)^0.75 * 0.0006 * 1E2");
         map("(V_fat * 1000 / 14.2)^0.75 * 5 * 1E4", "(V_fat * 1000 / 14.2)^0.75 * 5 * 1E2");
         map("(V_hrt * 1000 / 1.2)^0.75 * 7.54 * 1E4", "(V_hrt * 1000 / 1.2)^0.75 * 7.54 * 1E2");
         map("(V_kid * 1000 / 7)^0.75 * 1000 * 1E4", "(V_kid * 1000 / 7)^0.75 * 1000 * 1E2");
         map("((V_lin-V_Mucosa) * 1000 / 11.1)^0.75 * 1000 * 1E4", "((V_lin-V_Mucosa) * 1000 / 11.1)^0.75 * 1000 * 1E2");
         map("(V_liv * 1000 / 10)^0.75 * 82 * 1E4", "(V_liv * 1000 / 10)^0.75 * 82 * 1E2");
         map("(V_lng * 1000  / 2.2)^0.75 * 0.096 * 1E4", "(V_lng * 1000  / 2.2)^0.75 * 0.096 * 1E2");
         map("(V_mus * 1000 / 110.1)^0.75 * 7.54 * 1E4", "(V_mus * 1000 / 110.1)^0.75 * 7.54 * 1E2");
         map("(V_pan * 1000 / 1.3)^0.75 * 1000 * 1E4", "(V_pan * 1000 / 1.3)^0.75 * 1000 * 1E2");
         map("((V_sin-V_Mucosa) * 1000 / 11.1)^0.75 * 1000 * 1E4", "((V_sin-V_Mucosa) * 1000 / 11.1)^0.75 * 1000 * 1E2");
         map("(V_skn * 1000 / 43.4)^0.75 * 0.12 * 1E4", "(V_skn * 1000 / 43.4)^0.75 * 0.12 * 1E2");
         map("(V_spl * 1000 / 1.3)^0.75 * 1000 * 1E4", "(V_spl * 1000 / 1.3)^0.75 * 1000 * 1E2");
         map("(V_sto * 1000 / 1.1)^0.75 * 1000 * 1E4", "(V_sto * 1000 / 1.1)^0.75 * 1000 * 1E2");
         map("(V_tes * 1000 / 2.5)^0.75 * 2 * 1E4", "(V_tes * 1000 / 2.5)^0.75 * 2 * 1E2");
         map("HCT * A_to_V_bc * 0.6 * V * f_vas * 1000", "HCT * A_to_V_bc * 0.6 * V * f_vas");
         map("HCT * A_to_V_bc * 0.6 * V * 1000", "HCT * A_to_V_bc * 0.6 * V");
         map("HCT * A_to_V_bc * 0.6 * (V-V_Mucosa) * f_vas * 1000", "HCT * A_to_V_bc * 0.6 * (V-V_Mucosa) * f_vas");
         map("Area*d/1000", "Area*d");
         map("(TabletIsActive = 1) AND (DrugMass <= 4/3 * N * rho * Pi * (ParticleRadiusDissolved*1E-7)^3 * 1E6 / MW)", "(TabletIsActive = 1) AND (DrugMass <= 4/3 * N * rho * Pi * (ParticleRadiusDissolved)^3 / MW)");
         map("SA_bc /1000 * fu * (P_pls_rbc * C_pls - P_rbc_pls * C_bc  /K_rbc)", "SA_bc * fu * (P_pls_rbc * C_pls - P_rbc_pls * C_bc  /K_rbc)");
         map("fu * P_endothelial * SA /1000 * (C_pls - C_int / K_int_pls)", "fu * P_endothelial * SA * (C_pls - C_int / K_int_pls)");
         map("(J_iso+alpha_l*Q_lymph)*(1-sigma_l)/(P_endothelial_l*SA_pls_int/1000)", "(J_iso+alpha_l*Q_lymph)*(1-sigma_l)/(P_endothelial_l*SA_pls_int)");
         map("(-J_iso+(1-alpha_l)*Q_lymph)*(1-sigma_s)/(P_endothelial_s*SA_pls_int/1000)", "(-J_iso+(1-alpha_l)*Q_lymph)*(1-sigma_s)/(P_endothelial_s*SA_pls_int)");
         map("(J_iso+alpha_l*Q_lymph)*(1-sigma_l)*C_pl+(P_endothelial_l*SA_pls_int/1000)*(C_pl-C_int/K_int_pls)*Pe_l/(exp(Pe_l)-1) + (-J_iso+(1-alpha_l)*Q_lymph)*(1-sigma_s)*C_pl+(P_endothelial_s*SA_pls_int/1000)*(C_pl-C_int/K_int_pls)*Pe_s/(exp(Pe_s)-1)", "(J_iso+alpha_l*Q_lymph)*(1-sigma_l)*C_pl+(P_endothelial_l*SA_pls_int)*(C_pl-C_int/K_int_pls)*Pe_l/(exp(Pe_l)-1) + (-J_iso+(1-alpha_l)*Q_lymph)*(1-sigma_s)*C_pl+(P_endothelial_s*SA_pls_int)*(C_pl-C_int/K_int_pls)*Pe_s/(exp(Pe_s)-1)");
         map("f_endo*SA_pls_int*d_endo/1000", "f_endo*SA_pls_int*d_endo");
         map("PI*L_cae*r_cae*r_cae/1000", "PI*L_cae*r_cae*r_cae");
         map("PI*L_duo/3*(r_duo*r_duo+r_duo*r_uje+r_uje*r_uje)/1000", "PI*L_duo/3*(r_duo*r_duo+r_duo*r_uje+r_uje*r_uje)");
         map("PI*L_lil/3*(r_lil*r_lil+r_lil*r_icj+r_icj*r_icj)/1000", "PI*L_lil/3*(r_lil*r_lil+r_lil*r_icj+r_icj*r_icj)");
         map("PI*L_lje/3*(r_lje*r_lje+r_lje*r_uil+r_uil*r_uil)/1000", "PI*L_lje/3*(r_lje*r_lje+r_lje*r_uil+r_uil*r_uil)");
         map("PI*L_uil/3*(r_uil*r_uil+r_uil*r_lil+r_lil*r_lil)/1000", "PI*L_uil/3*(r_uil*r_uil+r_uil*r_lil+r_lil*r_lil)");
         map("PI*L_uje/3*(r_uje*r_uje+r_uje*r_lje+r_lje*r_lje)/1000", "PI*L_uje/3*(r_uje*r_uje+r_uje*r_lje+r_lje*r_lje)");
         map("PI*(r1*r1+r1*r2+r2*r2)/3*L/1000", "PI*(r1*r1+r1*r2+r2*r2)/3*L");
         map("SA_pls_int*d_endo/1000", "SA_pls_int*d_endo");

         //no change in this formula but dimension has changed and we do not want to override the formula
         map("Aeff_duo + Aeff_uje + Aeff_lje + Aeff_uil + Aeff_lil");
         map("AeffFactor*Pi*L*(r1+r1)");
         map("AeffFactor*Pi*L*(r1+r2)");
         map("Ageom * AeffFactor / MicrovilliFactor");
         map("DefaultThickness * Volume / Default_Volume");
         map("DosePerBodyWeight * BW");
         map("k * (Q_org / ((V_org-V_Mucosa) * f_vas_org)) ^ beta");
         map("k * (Q_org / (V_org * f_vas_org)) ^ beta");
         map("k * f_vas_org * (V_org-V_Mucosa)");
         map("k * f_vas_org * V_org");
         map("k * Q_org ^ beta");
         map("L_duo + L_uje + L_lje + L_uil + L_lil");
         map("L_p0 + Height*L_p1");
         map("P * P_cell_int_factor");
         map("P * P_int_cell_factor");
         map("P * P_pls_rbc_factor");
         map("P * P_rbc_pls_factor");
         map("P/2");
         map("P_int_InVitro");
         map("PI*(r1+r2)*L");
         map("r1_p0 + Height*r1_p1");
         map("r2_p0 + Height*r2_p1");
         map("SA_pls_int_bon+SA_pls_int_brn+SA_pls_int_fat+SA_pls_int_tes+SA_pls_int_hrt+SA_pls_int_kid+SA_pls_int_lin+SA_pls_int_liv+SA_pls_int_lng+SA_pls_int_mus+SA_pls_int_pan+SA_pls_int_skn+SA_pls_int_sin+SA_pls_int_spl+SA_pls_int_sto");
         map("Solubility * Solubility_pKa_REFpH_Factor / Solubility_pKa_pH_Factor");
         map("TabletIsActive AND OralApplicationsEnabled ? (w_fact*(w_sto*location_sto*FillLevelFlag_duo*(Time>StartTime+LagTime?1:0) +w_duo*location_duo*FillLevelFlag_uje +w_uje*location_uje*FillLevelFlag_lje +w_lje*location_lje*FillLevelFlag_uil+w_uil*location_uil*F");
         map("Thickness_p1 + Thickness_p2*(1-exp(Thickness_p3*Height))");
         map("(C_bon*V_bon+C_brn*V_brn+C_fat*V_fat+C_tes*V_tes+C_hrt*V_hrt+C_kid*V_kid+C_lin*V_lin+C_liv*V_liv+C_lng*V_lng+C_mus*V_mus+C_pan*V_pan+C_skn*V_skn+C_sin*V_sin+C_spl*V_spl+C_sto*V_sto)/(V_bon+V_brn+V_fat+V_tes+V_hrt+V_kid+V_lin+V_liv+V_lng+V_mus+V_pan+V_skn+V_sin+V_spl+V_sto)");

         //special case for MoBi
         map("0.006 * BW / 73");
         map("V_art + V_sto + V_sin + V_lin + V_pan + V_spl + V_liv + V_lng + V_mus + V_fat + V_skn + V_kid + V_brn + V_hrt + V_bon + V_tes + V_pve + V_ven");
         map("(Q_liv+Q_pve - CL_pls / B2P* BW >0)  ? CL_pls * BW * (Q_liv+Q_pve) / (fu  * (Q_liv+Q_pve - CL_pls / B2P* BW) * V * f_cell) : 1000000");
         map("AmountOfWaterPerBodyWeight * BW");
      }

      private void map(string formula)
      {
         map(formula, formula);
      }
      private void map(string oldFormula, string newFormula)
      {
         _formulaCache.Add(removeSpace(oldFormula), newFormula);
      }

      public string NewFormulaFor(string oldFormula)
      {
         return _formulaCache[removeSpace(oldFormula)];
      }

      public bool FormulaWasConverted(string newFormula)
      {
         return _formulaCache.Any(f => string.Equals(f, newFormula));
      }

      private string removeSpace(string formula)
      {
         var trim = formula.Trim();
         trim = trim.Replace(" ", "");
         return trim;
      }
   }
}