# VaccinationApp
### Information
Bachelor's thesis on system of selection of optimal vaccination schemes of population using epidemiological data.
This software product is created for the purpose of bachelor`s work and is not intended for commercial use. 
This application was created when I studied at the university at the fourth course in 2017-2018.

C # programming language and Microsoft Visual Studio 2017 programming environment were used to develop the appliction. 
A WinWorms framework was used to develop the GUI. 
The appliction was designed for use with Windows 10, Windows 8, Windows 7 and Windows XP SP3.

The problem of the spread of various infectious diseases and epidemics is important for all mankind. 
Rotavirus infection (RVI) is a major cause of acute viral gastroenteritis in young children and high rates of 
childhood morbidity and mortality worldwide. Vaccination is used to reduce the spread of infection. 
Given the scarcity of resources, it is not possible to vaccinate the entire population, 
so the task of determining the optimal public and individual costs for vaccination and treatment is and will remain relevant.

The aim of the thesis is to develop a system of selection of optimal vaccination schemes of population using epidemiological data.

The software uses epidemiological models that allow us to study the spread of infectious diseases, 
make a forecast for the future, determine the effectiveness of vaccination, select the optimal vaccination scheme using 
epidemiological data. 
Due to the use of epidemiological models it is possible to prevent epidemics of infectious diseases in a timely manner.

The system has a software architecture that consists of three layers: data access layer, business logic layer, 
and presentation layer. 

The data access layer is responsible for reading, processing and storing input epidemiological data. 
The processing of input epidemiological data is necessary to eliminate excess data and automate the process of collecting 
and calculating statistics.

The business logic layer builds epidemiological models, makes a forecast for the future. 
The module determines the optimal vaccination regimens depending on the epidemiological input.

The presentation layer uses a graphical user interface to output the results of the system.

The developed system can be used for RVI research, disease prognosis, selection of optimal vaccination schemes, 
and determination of costs for treatment and vaccination.

### Functionality of the Application
During the completion of the bachelor's thesis, a system of selection of optimal vaccination schemes 
of population using epidemiological data, which performs the following functions:
1) processing and conversion of input data;
2) parameterization of the epidemiological SIS model, which includes finding unknown parameters;
3) construction of the population morbidity forecast for a certain period of time;
4) projection of population change;
5) making a forecast of population change taking into account the age distribution of the population;
6) construction of the epidemiological SIS model without taking into account the age structure of the population;
7) construction of an epidemiological SIS model based on the age structure of the population;
8) construction and parameterization of the cohort model;
9) construction of an epidemiological SIS model with vaccination without taking into account the age structure 
of the population;
10) construction of an epidemiological SIS model with vaccination taking into account the age structure of the population;
11) evaluation of vaccine prophylaxis efficacy taking into account unstable fertility and mortality (prognosis), 
impact on cohort model;
12) analysis of the effectiveness and instability of vaccination at different values of the costs of vaccination 
and treatment, both for one person and for society (equality of two optima);
13) carrying out an analysis of the sensitivity of changes in the incidence of the population, 
coverage of newborns by vaccination from the costs of vaccination and treatment;
14) solving the problem of vaccine optimization: when to vaccinate, and when not, the optimal level of individual 
and public costs for vaccination and treatment, the optimal level of coverage for newborns, 
the choice of the optimal individual vaccination strategy;
15) saving the results of the system.

### Usage instruction
After selecting input files, it may take some time to read the files, so wait.
