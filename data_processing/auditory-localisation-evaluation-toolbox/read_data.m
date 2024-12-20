
filelistReg = ["data/subject0";
            "data/subject1";
            "data/subject2";
            "data/subject3";
            "data/subject4";
            "data/subject5";
            "data/subject6";
            "data/subject7";
            "data/subject8";
            "data/subject9";
            "data/subject10";
            "data/subject11";
            ];

filelistPer = ["data/subject0-personal";
            "data/subject1-personal";
            "data/subject2-personal";
            "data/subject3-personal";
            "data/subject4-personal";
            "data/subject5-personal";
            "data/subject6-personal";
            "data/subject7-personal";
            "data/subject8-personal";
            "data/subject9-personal";
            "data/subject10-personal";
            "data/subject11-personal";
            ];


greatCircError = [];
greatCircErrorLocal = [];
errorAzi = [];
errorAziSign = [];
errorEle = [];
errorEleSign = [];
precisionRatio = {};
frontBackRatio = {};
upDownRatio = {};
leftRightRatio = {};
combinedRatio = {};


for i = 1:length(filelistReg)

    g = readcell(filelistReg(i));
    
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
    
    greatCircError = cat(1, greatCircError, results.greatCircAngle);
    greatCircErrorLocal = cat(1, greatCircErrorLocal, localErrors);

    errorAzi = cat(1, errorAzi, localAzi);
    errorAziSign = cat(1, errorAziSign, localAziSigned);

    errorEle = cat(1, errorEle, localEle);
    errorEleSign = cat(1, errorEleSign, localEleSigned);

    precisionRatio = [precisionRatio, sum(precision) / 47];
    frontBackRatio = [frontBackRatio, sum(frontBack) / 47];
    upDownRatio = [upDownRatio, sum(upDown) / 47];
    leftRightRatio = [leftRightRatio, sum(leftRight) / 47];
    combinedRatio = [combinedRatio, sum(combined) / 47];

end

greatCircErrorPer = [];
greatCircErrorLocalPer = [];
errorAziPer = [];
errorAziSignPer = [];
errorElePer = [];
errorEleSignPer = [];

precisionRatioPer = {};
frontBackRatioPer = {};
upDownRatioPer = {};
leftRightRatioPer = {};
combinedRatioPer = {};

for i = 1:length(filelistPer)

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
    
    greatCircErrorPer = cat(1, greatCircErrorPer, results.greatCircAngle);
    greatCircErrorLocalPer = cat(1, greatCircErrorLocalPer, localErrors);

    errorAziPer = cat(1, errorAziPer, localAzi);
    errorAziSignPer = cat(1, errorAziSignPer, localAziSigned);

    errorElePer = cat(1, errorElePer, localEle);
    errorEleSignPer = cat(1, errorEleSignPer, localEleSigned);

    precisionRatioPer = [precisionRatioPer, sum(precision) / 47];
    frontBackRatioPer = [frontBackRatioPer, sum(frontBack) / 47];
    upDownRatioPer = [upDownRatioPer, sum(upDown) / 47];
    leftRightRatioPer = [leftRightRatioPer, sum(leftRight) / 47];
    combinedRatioPer = [combinedRatioPer, sum(combined) / 47];

end


% Error box plot
d = rmoutliers(errorAzi, 'mean');
e = rmoutliers(errorAziPer, 'mean');
C = {greatCircErrorLocal(:), greatCircErrorLocalPer(:), d(:), e(:), errorEle(:), errorElePer(:)};

maxNumEl = max(cellfun(@numel,C));
Cpad = cellfun(@(x){padarray(x(:),[maxNumEl-numel(x),0],NaN,'post')}, C);
Cmat = cell2mat(Cpad);

group = [1,2,3,4,5,6];
positions = [1 1.25 1.5 1.75 2 2.25];
boxplot(Cmat, group, 'Positions',positions);
ylabel('Error angle in degrees')
title("Local localization errors (n=12)")

set(gca,'xtick',[mean(positions(1:2)) mean(positions(3:4)) mean(positions(5:6)) ])
set(gca,'xticklabel',{'Local circle error', 'Azimuth error', 'Elevation error'})

color = ['c', 'y', 'c', 'y', 'c', 'y', 'c', 'y'];
h = findobj(gca,'Tag','Box');
for j=1:length(h)
   patch(get(h(j),'XData'),get(h(j),'YData'),color(j),'FaceAlpha',.5);
end

c = get(gca, 'Children');

hleg1 = legend(c(1:2), 'Non-personal', 'Personal' );
saveas(gcf,'ErrorBar.png')



% Bias box plot
f = rmoutliers(errorAziSign, 'mean');
g = rmoutliers(errorAziSignPer, 'mean');
C = {f(:), g(:), errorEleSign(:), errorEleSignPer(:)};

maxNumEl = max(cellfun(@numel,C));
Cpad = cellfun(@(x){padarray(x(:),[maxNumEl-numel(x),0],NaN,'post')}, C);
Cmat = cell2mat(Cpad);


group = [1,2,3,4];
positions = [1 1.25 1.5 1.75];
boxplot(Cmat, group, 'Positions',positions);
ylabel('Error angle in degrees')
title("Local signed localization errors (n=12)")

set(gca,'xtick',[mean(positions(1:2)) mean(positions(3:4))])
set(gca,'xticklabel',{'Signed azimuth error', 'Signed elevation error'})

color = ['c', 'y', 'c', 'y', 'c', 'y', 'c', 'y'];
h = findobj(gca,'Tag','Box');
for j=1:length(h)
   patch(get(h(j),'XData'),get(h(j),'YData'),color(j),'FaceAlpha',.5);
end

c = get(gca, 'Children');

hleg1 = legend(c(1:2), 'Non-personal', 'Personal' );
saveas(gcf,'BiasBar.png')

% Global circ error
C = {greatCircError, greatCircErrorPer};

maxNumEl = max(cellfun(@numel,C));
Cpad = cellfun(@(x){padarray(x(:),[maxNumEl-numel(x),0],NaN,'post')}, C);
Cmat = cell2mat(Cpad);

group = [1,2];
positions = [1 1.1];
boxplot(Cmat, group, 'Positions',positions);
ylabel('Error angle in degrees')
title("Global localization errors")

set(gca,'xtick',[mean(positions(1:2))])
set(gca,'xticklabel',{'Global great circle error'})

color = ['c', 'y', 'c', 'y', 'c', 'y', 'c', 'y'];
h = findobj(gca,'Tag','Box');
for j=1:length(h)
   patch(get(h(j),'XData'),get(h(j),'YData'),color(j),'FaceAlpha',.5);
end

c = get(gca, 'Children');

hleg1 = legend(c(1:2), 'Non-personal', 'Personal', 'Location', 'northwest');
saveas(gcf,'GlobalError.png')




disp(mean(cell2mat(precisionRatio)))
disp(mean(cell2mat(frontBackRatio)))
disp(mean(cell2mat(upDownRatio)))
disp(mean(cell2mat(leftRightRatio)))
disp(mean(cell2mat(combinedRatio)))

disp(mean(cell2mat(precisionRatioPer)))
disp(mean(cell2mat(frontBackRatioPer)))
disp(mean(cell2mat(upDownRatioPer)))
disp(mean(cell2mat(leftRightRatioPer)))
disp(mean(cell2mat(combinedRatioPer)))






