% This script is an example how to start a parameter identification with
% the file exported by PK-Sim or MoBi
% Here the problem is solved by the nelder mead algorithm of matlab, 
% but this file can be adjusted for your own customized alogorithm and errorfunctional. 
% For that the MoBi Toolbox for Matlab provides two functions which
% accept as input the structure PI, generated in this script:
% getPIErrorFunctionalForOSPSuiteExport
% getPIWeightedResidualsForOSPSuiteExport.


% Name of file that contains the definition of the identification problem
PI_xml = @PI_FILE_NAME;

% List of corresponding simulations
simulationList = {@SIM_FILE_NAMES};


% read xml and initialize simulations with corresponding parameters
PI=initParameterIdentificationForOSPSuiteExport(PI_xml,simulationList);
 
% start the algorithm
finalValues = fminsearch(@(p) getPIErrorFunctionalForOSPSuiteExport(p,PI),[PI.par(:).startValue]);

% create default output figures for the parameter identification
plotPIDefaultOutputForOSPSuiteExport(PI,finalValues);