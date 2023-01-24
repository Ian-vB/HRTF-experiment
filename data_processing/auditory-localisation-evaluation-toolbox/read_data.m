
filelistReg = ["data/subject1";
            "data/subject2";
            "data/subject3";
            "data/subject4";
            "data/subject5";
            "data/subject6";
            "data/subject7";
            "data/subject8";
            ];

filelistPer = ["data/subject1-personal";
            "data/subject2-personal";
            "data/subject3-personal";
            "data/subject4-personal";
            "data/subject5-personal";
            "data/subject6-personal";
            "data/subject7-personal";
            "data/subject8-personal";
            ];


greatCircError = {};
greatCircErrorLocal = {};
errorAzi = {};
errorAziSign = {};
errorEle = {};
errorEleSign = {};
precisionRatio = {};
frontBackRatio = {};
upDownRatio = {};
leftRightRatio = {};
combinedRatio = {};


for i = 2:length(filelistPer)

    g = readcell(filelistPer(i));
    
    spawn = g(1:47, 1:3);
    hit = g(1:47, 4:6);
    spawn = cell2mat(spawn);
    hit = cell2mat(hit);
    
    results = getErrors(spawn, hit);
    
    precision = results.confusionStr.poirier == "precision";
    frontBack = results.confusionStr.poirier == "front-back";
    upDown = results.confusionStr.poirier == "up-down";
    leftRight = results.confusionStr.poirier == "left-right";
    combined = results.confusionStr.poirier == "combined";

    localErrors = results.greatCircAngle(precision);

    azi = results.errorSph(1:end, 1);
    localAzi = azi(precision);
    aziSigned = results.errorSphSigned(1:end, 1);
    localAziSigned = aziSigned(precision);

    ele = results.errorSph(1:end, 2);
    localEle = ele(precision);
    eleSigned = results.errorSphSigned(1:end, 2);
    localEleSigned = eleSigned(precision);
    
    greatCircError = [greatCircError, mean(results.greatCircAngle)];
    greatCircErrorLocal = [greatCircErrorLocal, mean(localErrors)];

    errorAzi = [errorAzi, mean(localAzi)];
    errorAziSign = [errorAziSign, mean(localAziSigned)];

    errorEle = [errorEle, mean(localEle)];
    errorEleSign = [errorEleSign, mean(localEleSigned)];

    precisionRatio = [precisionRatio, sum(precision) / 47];
    frontBackRatio = [frontBackRatio, sum(frontBack) / 47];
    upDownRatio = [upDownRatio, sum(upDown) / 47];
    leftRightRatio = [leftRightRatio, sum(leftRight) / 47];
    combinedRatio = [combinedRatio, sum(combined) / 47];

end

disp(mean(cell2mat(greatCircError)));
[h,p, ci, stats] = ttest(cell2mat(greatCircError));
disp(std(cell2mat(greatCircError)));

% disp(mean(cell2mat(greatCircErrorLocal)))
% disp(mean(cell2mat(errorAzi)))
% disp(mean(cell2mat(errorAziSign)))
% disp(mean(cell2mat(errorEle)))
% disp(mean(cell2mat(errorEleSign)))
% disp(mean(cell2mat(precisionRatio)))
% disp(mean(cell2mat(frontBackRatio)))
% disp(mean(cell2mat(upDownRatio)))
% disp(mean(cell2mat(leftRightRatio)))
% disp(mean(cell2mat(combinedRatio)))




