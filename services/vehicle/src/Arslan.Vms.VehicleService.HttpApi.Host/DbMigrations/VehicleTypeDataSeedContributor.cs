﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.Vehicles.VehicleTypes
{
    public class VehicleTypeDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        private readonly IGuidGenerator _guidGenerator;
        List<VehicleDataSeed> _vehicles;
        private readonly ICurrentTenant _currentTenant;

        public VehicleTypeDataSeedContributor(
            IVehicleTypeRepository vehicleTypeRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            _guidGenerator = guidGenerator;
            _vehicleTypeRepository = vehicleTypeRepository;
            _vehicles = new List<VehicleDataSeed>();
            _currentTenant = currentTenant;
        }

        public virtual async Task SeedAsync(DataSeedContext context)
        {
            var ls = ListVehicleTypes.TrimEnd(';').Trim('\r').Trim('\n').Split(';');
            var vehicleModels = new List<VehicleDataSeedModel>();

            foreach (var item in ls)
            {
                var x = item.Split(",", StringSplitOptions.RemoveEmptyEntries);
                vehicleModels.Add(new VehicleDataSeedModel(x[0].Trim('\r').Trim('\n'), x[1], x[2]));
            }

            var data = vehicleModels.GroupBy(g => new { g.ModelType, g.Brand }).Select(s => new { s.Key.ModelType, s.Key.Brand, models = s.Select(s => s.Model).ToList() });

            foreach (var item in data)
            {
                _vehicles.Add(new VehicleDataSeed { ModelType = item.ModelType.TrimStart("\r\n".ToCharArray()), Brand = item.Brand, Models = item.models });
            }

            var vehicleTypes = (await _vehicleTypeRepository.GetQueryableAsync()).ToList();
            var batchInsert = new List<VehicleType>();

            foreach (var _vehicleType in _vehicles.Select(s => s.ModelType).Distinct())
            {
                Guid vehicleTypeId;
                if (!vehicleTypes.Any(a => a.Name == _vehicleType && a.Level == 1 && a.ParentId == null))
                {
                    var vehicleType = new VehicleType(_guidGenerator.Create(), _currentTenant.Id, null, _vehicleType, 1);
                    batchInsert.Add(vehicleType);
                    vehicleTypeId = vehicleType.Id;
                }
                else
                {
                    vehicleTypeId = vehicleTypes.FirstOrDefault(a => a.Name == _vehicleType && a.Level == 1 && a.ParentId == null).Id;
                }


                foreach (var vehicle in _vehicles.Where(w => w.ModelType == _vehicleType))
                {
                    Guid brandId;
                    if (!vehicleTypes.Any(a => a.Name == vehicle.Brand && a.Level == 2 && a.ParentId == vehicleTypeId))
                    {
                        var brand = new VehicleType(_guidGenerator.Create(), _currentTenant.Id, vehicleTypeId, vehicle.Brand, 2);
                        batchInsert.Add(brand);
                        brandId = brand.Id;
                    }
                    else
                    {
                        brandId = vehicleTypes.FirstOrDefault(a => a.Name == vehicle.Brand && a.Level == 2 && a.ParentId == vehicleTypeId).Id;
                    }

                    foreach (var _model in vehicle.Models)
                    {
                        if (!vehicleTypes.Any(a => a.Name == _model && a.Level == 3 && a.ParentId == brandId))
                        {
                            var model = new VehicleType(_guidGenerator.Create(), _currentTenant.Id, brandId, _model, 3);
                            batchInsert.Add(model);
                        }
                    }
                }
            }

            await _vehicleTypeRepository.AddRangeAsync(batchInsert);
        }

        public class VehicleDataSeedModel
        {
            public VehicleDataSeedModel()
            {

            }

            public VehicleDataSeedModel(string modelType, string brand, string model)
            {
                ModelType = modelType;
                Brand = brand;
                Model = model;
            }

            public string Brand { get; set; }
            public string Model { get; set; }
            public string ModelType { get; set; }
        }

        public class VehicleDataSeed
        {
            public string Brand { get; set; }
            public string ModelType { get; set; }
            public List<string> Models { get; set; }

            public VehicleDataSeed()
            {
                Models = new List<string>();
            }
        }

        public static class ModelTypeConst
        {
            public static string Cars = "Cars";
            public static string SUVPickup = "SUV";
            public static string MinivanPanelvan = "Minivan";
        }

        public static string ListVehicleTypes =
@$"
Cars,Acura,CL;
Cars,Acura,Integra;
Cars,Acura,Legend;
Cars,Acura,MDX;
Cars,Acura,NSX;
Cars,Acura,RDX;
Cars,Acura,RL;
Cars,Acura,RSX;
Cars,Acura,SLX;
Cars,Acura,TL;
Cars,Acura,TSX;
Cars,Acura,Vigor;
Cars,Acura,ZDX;
Cars,Alfa Romeo,164;
Cars,Alfa Romeo,Spider;
Cars,Aptera,2e;
Cars,Aptera,Typ-1;
Cars,Aptera,Type-1h;
Cars,Aston Martin,DB9 Volante;
Cars,Aston Martin,DB9;
Cars,Aston Martin,DBS;
Cars,Aston Martin,Rapide;
Cars,Aston Martin,V12 Vantage;
Cars,Aston Martin,V8 Vantage S;
Cars,Aston Martin,V8 Vantage;
Cars,Aston Martin,Vanquish S;
Cars,Aston Martin,Vantage;
Cars,Aston Martin,Virage;
Cars,Audi,100;
Cars,Audi,200;
Cars,Audi,4000;
Cars,Audi,4000CS Quattro;
Cars,Audi,4000s Quattro;
Cars,Audi,4000s;
Cars,Audi,5000CS Quattro;
Cars,Audi,5000CS;
Cars,Audi,5000S;
Cars,Audi,80/90;
Cars,Audi,80;
Cars,Audi,90;
Cars,Audi,A3;
Cars,Audi,A4;
Cars,Audi,A5;
Cars,Audi,A6;
Cars,Audi,A7;
Cars,Audi,A8;
Cars,Audi,Allroad;
Cars,Audi,Cabriolet;
Cars,Audi,Coupe GT;
Cars,Audi,Coupe Quattro;
Cars,Audi,Q5;
Cars,Audi,Q7;
Cars,Audi,Quattro;
Cars,Audi,R8;
Cars,Audi,riolet;
Cars,Audi,RS 4;
Cars,Audi,RS 6;
Cars,Audi,RS4;
Cars,Audi,RS6;
Cars,Audi,S4;
Cars,Audi,S5;
Cars,Audi,S6;
Cars,Audi,S8;
Cars,Audi,TT;
Cars,Audi,V8;
Cars,Austin,Mini Cooper S;
Cars,Austin,Mini Cooper;
Cars,Austin,Mini;
Cars,Bentley,Arnage;
Cars,Bentley,Azure T;
Cars,Bentley,Azure;
Cars,Bentley,Brooklands;
Cars,Bentley,Continental Flying Spur;
Cars,Bentley,Continental GT;
Cars,Bentley,Continental GTC;
Cars,Bentley,Continental Super;
Cars,Bentley,Continental;
Cars,Bentley,Mulsanne;
Cars,BMW,1 Series;
Cars,BMW,3 Series;
Cars,BMW,325;
Cars,BMW,330;
Cars,BMW,5 Series;
Cars,BMW,525;
Cars,BMW,530;
Cars,BMW,545;
Cars,BMW,550;
Cars,BMW,6 Series;
Cars,BMW,600;
Cars,BMW,645;
Cars,BMW,650;
Cars,BMW,7 Series;
Cars,BMW,745;
Cars,BMW,750;
Cars,BMW,760;
Cars,BMW,8 Series;
Cars,BMW,Alpina B7;
Cars,BMW,M Roadster;
Cars,BMW,M;
Cars,BMW,M3;
Cars,BMW,M5;
Cars,BMW,M6;
Cars,BMW,X3;
Cars,BMW,X5 M;
Cars,BMW,X5;
Cars,BMW,X6 M;
Cars,BMW,X6;
Cars,BMW,Z3;
Cars,BMW,Z4 M Roadster;
Cars,BMW,Z4 M;
Cars,BMW,Z4;
Cars,BMW,Z8;
Cars,Bugatti,Veyron;
Cars,Buick,Century;
Cars,Buick,Coachbuilder;
Cars,Buick,Electra;
Cars,Buick,Enclave;
Cars,Buick,Estate;
Cars,Buick,LaCrosse;
Cars,Buick,LeSabre;
Cars,Buick,Lucerne;
Cars,Buick,Park Avenue;
Cars,Buick,Rainier;
Cars,Buick,Reatta;
Cars,Buick,Regal;
Cars,Buick,Rendezvous;
Cars,Buick,Riviera;
Cars,Buick,Roadmaster;
Cars,Buick,Skyhawk;
Cars,Buick,Skylark;
Cars,Buick,Somerset;
Cars,Buick,Special;
Cars,Buick,Terraza;
Cars,Buick,Verano;
Cars,Cadillac,Allante;
Cars,Cadillac,Brougham;
Cars,Cadillac,Catera;
Cars,Cadillac,CTS;
Cars,Cadillac,CTS-V;
Cars,Cadillac,DeVille;
Cars,Cadillac,DTS;
Cars,Cadillac,Eldorado;
Cars,Cadillac,Escalade ESV;
Cars,Cadillac,Escalade EXT;
Cars,Cadillac,Escalade;
Cars,Cadillac,Fleetwood;
Cars,Cadillac,Seville;
Cars,Cadillac,Sixty Special;
Cars,Cadillac,SRX;
Cars,Cadillac,STS;
Cars,Cadillac,STS-V;
Cars,Cadillac,XLR;
Cars,Cadillac,XLR-V;
Cars,Chevrolet,1500;
Cars,Chevrolet,2500;
Cars,Chevrolet,3500;
Cars,Chevrolet,APV;
Cars,Chevrolet,Astro;
Cars,Chevrolet,Avalanche 1500;
Cars,Chevrolet,Avalanche 2500;
Cars,Chevrolet,Avalanche;
Cars,Chevrolet,Aveo;
Cars,Chevrolet,Bel Air;
Cars,Chevrolet,Beretta;
Cars,Chevrolet,Blazer;
Cars,Chevrolet,Camaro;
Cars,Chevrolet,Caprice Classic;
Cars,Chevrolet,Caprice;
Cars,Chevrolet,Cavalier;
Cars,Chevrolet,Citation;
Cars,Chevrolet,Classic;
Cars,Chevrolet,Cobalt SS;
Cars,Chevrolet,Cobalt;
Cars,Chevrolet,Colorado;
Cars,Chevrolet,Corsica;
Cars,Chevrolet,Corvair 500;
Cars,Chevrolet,Corvair;
Cars,Chevrolet,Corvette;
Cars,Chevrolet,Cruze;
Cars,Chevrolet,Equinox;
Cars,Chevrolet,Express 1500;
Cars,Chevrolet,Express 2500;
Cars,Chevrolet,Express 3500;
Cars,Chevrolet,Express;
Cars,Chevrolet,G-Series G10;
Cars,Chevrolet,G-Series G20;
Cars,Chevrolet,G-Series G30;
Cars,Chevrolet,HHR Panel;
Cars,Chevrolet,HHR;
Cars,Chevrolet,Impala SS;
Cars,Chevrolet,Impala;
Cars,Chevrolet,K5 Blazer;
Cars,Chevrolet,Lumina APV;
Cars,Chevrolet,Lumina;
Cars,Chevrolet,LUV;
Cars,Chevrolet,Malibu Maxx;
Cars,Chevrolet,Malibu;
Cars,Chevrolet,Metro;
Cars,Chevrolet,Monte Carlo;
Cars,Chevrolet,Monza;
Cars,Chevrolet,Prizm;
Cars,Chevrolet,S10 Blazer;
Cars,Chevrolet,S10;
Cars,Chevrolet,Silverado 1500;
Cars,Chevrolet,Silverado 2500;
Cars,Chevrolet,Silverado 3500;
Cars,Chevrolet,Silverado 3500HD;
Cars,Chevrolet,Silverado Hybrid;
Cars,Chevrolet,Silverado;
Cars,Chevrolet,Sonic;
Cars,Chevrolet,Sportvan G10;
Cars,Chevrolet,Sportvan G20;
Cars,Chevrolet,Sportvan G30;
Cars,Chevrolet,SSR;
Cars,Chevrolet,Suburban 1500;
Cars,Chevrolet,Suburban 2500;
Cars,Chevrolet,Suburban;
Cars,Chevrolet,Tahoe;
Cars,Chevrolet,Tracker;
Cars,Chevrolet,TrailBlazer;
Cars,Chevrolet,Traverse;
Cars,Chevrolet,Uplander;
Cars,Chevrolet,Vega;
Cars,Chevrolet,Venture;
Cars,Chevrolet,Volt;
Cars,Chrysler,200;
Cars,Chrysler,300;
Cars,Chrysler,300C;
Cars,Chrysler,300M;
Cars,Chrysler,Aspen;
Cars,Chrysler,Cirrus;
Cars,Chrysler,Concorde;
Cars,Chrysler,Crossfire Roadster;
Cars,Chrysler,Crossfire;
Cars,Chrysler,Fifth Ave;
Cars,Chrysler,Grand Voyager;
Cars,Chrysler,Imperial;
Cars,Chrysler,LeBaron;
Cars,Chrysler,LHS;
Cars,Chrysler,New Yorker;
Cars,Chrysler,Pacifica;
Cars,Chrysler,Prowler;
Cars,Chrysler,PT Cruiser;
Cars,Chrysler,Sebring;
Cars,Chrysler,Town & Country;
Cars,Chrysler,Voyager;
Cars,Citroën,2CV;
Cars,Citroën,CX;
Cars,Citroën,SM;
Cars,Corbin,Sparrow;
Cars,Daewoo,Lanos;
Cars,Daewoo,Leganza;
Cars,Daewoo,Nubira;
Cars,Daihatsu,Charade;
Cars,Daihatsu,Rocky;
Cars,Dodge,Aries;
Cars,Dodge,Aspen;
Cars,Dodge,Avenger;
Cars,Dodge,Caliber;
Cars,Dodge,Caravan;
Cars,Dodge,Challenger;
Cars,Dodge,Charger;
Cars,Dodge,Colt;
Cars,Dodge,D150 Club;
Cars,Dodge,D150;
Cars,Dodge,D250 Club;
Cars,Dodge,D250;
Cars,Dodge,D350 Club;
Cars,Dodge,D350;
Cars,Dodge,Dakota Club;
Cars,Dodge,Dakota;
Cars,Dodge,Daytona;
Cars,Dodge,Durango;
Cars,Dodge,Dynasty;
Cars,Dodge,Grand Caravan;
Cars,Dodge,Intrepid;
Cars,Dodge,Journey;
Cars,Dodge,Magnum;
Cars,Dodge,Monaco;
Cars,Dodge,Neon;
Cars,Dodge,Nitro;
Cars,Dodge,Omni;
Cars,Dodge,Ram 1500 Club;
Cars,Dodge,Ram 1500;
Cars,Dodge,Ram 2500 Club;
Cars,Dodge,Ram 2500;
Cars,Dodge,Ram 3500 Club;
Cars,Dodge,Ram 3500;
Cars,Dodge,Ram 50;
Cars,Dodge,Ram Van 1500;
Cars,Dodge,Ram Van 2500;
Cars,Dodge,Ram Van 3500;
Cars,Dodge,Ram Van B150;
Cars,Dodge,Ram Van B250;
Cars,Dodge,Ram Van B350;
Cars,Dodge,Ram Wagon B150;
Cars,Dodge,Ram Wagon B250;
Cars,Dodge,Ram Wagon B350;
Cars,Dodge,Ram;
Cars,Dodge,Ramcharger;
Cars,Dodge,Shadow;
Cars,Dodge,Spirit;
Cars,Dodge,Sprinter;
Cars,Dodge,Stealth;
Cars,Dodge,Stratus;
Cars,Dodge,Viper RT/10;
Cars,Dodge,Viper;
Cars,Eagle,Premier;
Cars,Eagle,Summit;
Cars,Eagle,Talon;
Cars,Eagle,Vision;
Cars,Fairthorpe,Rockette;
Cars,Ferrari,430 Scuderia;
Cars,Ferrari,458 Italia;
Cars,Ferrari,599 GTB Fiorano;
Cars,Ferrari,612 Scaglietti;
Cars,Ferrari,California;
Cars,Ferrari,F430 Spider;
Cars,Ferrari,F430;
Cars,Ferrari,FF;
Cars,Fiat,500;
Cars,Fiat,Nuova 500;
Cars,Fiat,124 Spider;
Cars,Fiat,500 Ailesi;
Cars,Fiat,Albea;
Cars,Fiat,Bravo;
Cars,Fiat,Egea;
Cars,Fiat,Linea;
Cars,Fiat,Palio;
Cars,Fiat,Panda;
Cars,Fiat,Punto;
Cars,Fiat,126 Bis;
Cars,Fiat,Brava;
Cars,Fiat,Coupe;
Cars,Fiat,Idea;
Cars,Fiat,Marea;
Cars,Fiat,Multipla;
Cars,Fiat,Sedici;
Cars,Fiat,Siena;
Cars,Fiat,Stilo;
Cars,Fiat,Tempra;
Cars,Fiat,Tipo;
Cars,Fiat,Ulysse;
Cars,Fiat,Uno;
Cars,Fiat,Croma;
Cars,Fillmore,Fillmore;
Cars,Foose,Hemisfear;
Cars,Ford,Aerostar;
Cars,Ford,Aspire;
Cars,Ford,Bronco II;
Cars,Ford,Bronco;
Cars,Ford,Club Wagon;
Cars,Ford,C-MAX Hybrid;
Cars,Ford,Contour;
Cars,Ford,Country;
Cars,Ford,Courier;
Cars,Ford,Crown Victoria;
Cars,Ford,E150;
Cars,Ford,E250;
Cars,Ford,E-350 Super Duty Van;
Cars,Ford,E-350 Super Duty;
Cars,Ford,E350;
Cars,Ford,Econoline E150;
Cars,Ford,Econoline E250;
Cars,Ford,Econoline E350;
Cars,Ford,Edge;
Cars,Ford,Escape;
Cars,Ford,Escort ZX2;
Cars,Ford,Escort;
Cars,Ford,E-Series;
Cars,Ford,Excursion;
Cars,Ford,EXP;
Cars,Ford,Expedition EL;
Cars,Ford,Expedition;
Cars,Ford,Explorer Sport Trac;
Cars,Ford,Explorer Sport;
Cars,Ford,Explorer;
Cars,Ford,F150;
Cars,Ford,F-250 Super Duty;
Cars,Ford,F250;
Cars,Ford,F-350 Super Duty;
Cars,Ford,F350;
Cars,Ford,F450;
Cars,Ford,Fairlane;
Cars,Ford,Falcon;
Cars,Ford,Festiva;
Cars,Ford,Fiesta;
Cars,Ford,Five Hundred;
Cars,Ford,Flex;
Cars,Ford,Focus ST;
Cars,Ford,Focus;
Cars,Ford,Freestar;
Cars,Ford,Freestyle;
Cars,Ford,F-Series Super Duty;
Cars,Ford,F-Series;
Cars,Ford,Fusion;
Cars,Ford,Galaxie;
Cars,Ford,GT;
Cars,Ford,GT500;
Cars,Ford,Laser;
Cars,Ford,Lightning;
Cars,Ford,LTD Crown Victoria;
Cars,Ford,LTD;
Cars,Ford,Model T;
Cars,Ford,Mustang;
Cars,Ford,Probe;
Cars,Ford,Ranger;
Cars,Ford,Taurus X;
Cars,Ford,Taurus;
Cars,Ford,Tempo;
Cars,Ford,Th!nk;
Cars,Ford,Thunderbird;
Cars,Ford,Torino;
Cars,Ford,Transit Connect;
Cars,Ford,Windstar;
Cars,Ford,ZX2;
Cars,Geo,Metro;
Cars,Geo,Prizm;
Cars,Geo,Storm;
Cars,Geo,Tracker;
Cars,GMC,1500 Club Coupe;
Cars,GMC,1500;
Cars,GMC,2500 Club Coupe;
Cars,GMC,2500;
Cars,GMC,3500 Club Coupe;
Cars,GMC,3500;
Cars,GMC,Acadia;
Cars,GMC,Canyon;
Cars,GMC,Envoy XL;
Cars,GMC,Envoy XUV;
Cars,GMC,Envoy;
Cars,GMC,Jimmy;
Cars,GMC,Rally Wagon 1500;
Cars,GMC,Rally Wagon 2500;
Cars,GMC,Rally Wagon 3500;
Cars,GMC,Safari;
Cars,GMC,Savana 1500;
Cars,GMC,Savana 2500;
Cars,GMC,Savana 3500;
Cars,GMC,Savana Cargo Van;
Cars,GMC,Savana;
Cars,GMC,Sierra 1500;
Cars,GMC,Sierra 2500;
Cars,GMC,Sierra 2500HD;
Cars,GMC,Sierra 3500;
Cars,GMC,Sierra 3500HD;
Cars,GMC,Sierra Denali;
Cars,GMC,Sierra Hybrid;
Cars,GMC,Sierra;
Cars,GMC,Sonoma Club Coupe;
Cars,GMC,Sonoma Club;
Cars,GMC,Sonoma;
Cars,GMC,Suburban 1500;
Cars,GMC,Suburban 2500;
Cars,GMC,Terrain;
Cars,GMC,Vandura 1500;
Cars,GMC,Vandura 2500;
Cars,GMC,Vandura 3500;
Cars,GMC,Yukon Denali;
Cars,GMC,Yukon XL 1500;
Cars,GMC,Yukon XL 2500;
Cars,GMC,Yukon XL;
Cars,GMC,Yukon;
Cars,Hillman,Minx Magnificent;
Cars,Holden,Monaro;
Cars,Honda,Accord Crosstour;
Cars,Honda,Accord;
Cars,Honda,Civic GX;
Cars,Honda,Civic Si;
Cars,Honda,Civic;
Cars,Honda,Crosstour;
Cars,Honda,CR-V;
Cars,Honda,CR-X;
Cars,Honda,CR-Z;
Cars,Honda,del Sol;
Cars,Honda,Element;
Cars,Honda,FCX Clarity;
Cars,Honda,Fit;
Cars,Honda,Insight;
Cars,Honda,Odyssey;
Cars,Honda,Passport;
Cars,Honda,Pilot;
Cars,Honda,Prelude;
Cars,Honda,Ridgeline;
Cars,Honda,S2000;
Cars,HUMMER,H1;
Cars,Hummer,H2 SUT;
Cars,Hummer,H2 SUV;
Cars,HUMMER,H2;
Cars,Hummer,H3;
Cars,HUMMER,H3T;
Cars,Hyundai,Accent;
Cars,Hyundai,Azera;
Cars,Hyundai,Elantra;
Cars,Hyundai,Entourage;
Cars,Hyundai,Equus;
Cars,Hyundai,Excel;
Cars,Hyundai,Genesis Coupe;
Cars,Hyundai,Genesis;
Cars,Hyundai,HED-5;
Cars,Hyundai,Santa Fe;
Cars,Hyundai,Scoupe;
Cars,Hyundai,Sonata;
Cars,Hyundai,Tiburon;
Cars,Hyundai,Tucson;
Cars,Hyundai,Veloster;
Cars,Hyundai,Veracruz;
Cars,Hyundai,XG300;
Cars,Hyundai,XG350;
Cars,Infiniti,EX;
Cars,Infiniti,FX;
Cars,Infiniti,G;
Cars,Infiniti,G25;
Cars,Infiniti,G35;
Cars,Infiniti,G37;
Cars,Infiniti,I;
Cars,Infiniti,IPL G;
Cars,Infiniti,J;
Cars,Infiniti,JX;
Cars,Infiniti,M;
Cars,Infiniti,Q;
Cars,Infiniti,QX;
Cars,Infiniti,QX56;
Cars,Isuzu,Amigo;
Cars,Isuzu,Ascender;
Cars,Isuzu,Axiom;
Cars,Isuzu,Hombre Space;
Cars,Isuzu,Hombre;
Cars,Isuzu,i-280;
Cars,Isuzu,i-290;
Cars,Isuzu,i-350;
Cars,Isuzu,i-370;
Cars,Isuzu,Impulse;
Cars,Isuzu,i-Series;
Cars,Isuzu,Oasis;
Cars,Isuzu,Rodeo Sport;
Cars,Isuzu,Rodeo;
Cars,Isuzu,Space;
Cars,Isuzu,Stylus;
Cars,Isuzu,Trooper;
Cars,Isuzu,VehiCROSS;
Cars,Jaguar,S-Type;
Cars,Jaguar,XF;
Cars,Jaguar,XJ Series;
Cars,Jaguar,XJ;
Cars,Jaguar,XK Series;
Cars,Jaguar,XK;
Cars,Jaguar,X-Type;
Cars,Jeep,Cherokee;
Cars,Jeep,Comanche;
Cars,Jeep,Commander;
Cars,Jeep,Compass;
Cars,Jeep,Grand Cherokee;
Cars,Jeep,Liberty;
Cars,Jeep,Patriot;
Cars,Jeep,Wrangler;
Cars,Jensen,Interceptor;
Cars,Kia,Amanti;
Cars,Kia,Borrego;
Cars,Kia,Carens;
Cars,Kia,Forte;
Cars,Kia,Mohave/Borrego;
Cars,Kia,Optima;
Cars,Kia,Rio;
Cars,Kia,Rio5;
Cars,Kia,Rondo;
Cars,Kia,Sedona;
Cars,Kia,Sephia;
Cars,Kia,Sorento;
Cars,Kia,Soul;
Cars,Kia,Spectra;
Cars,Kia,Spectra5;
Cars,Kia,Sportage;
Cars,Lamborghini,Aventador;
Cars,Lamborghini,Countach;
Cars,Lamborghini,Diablo;
Cars,Lamborghini,Gallardo;
Cars,Lamborghini,Murciélago LP640;
Cars,Lamborghini,Murciélago;
Cars,Lamborghini,Reventón;
Cars,Land Rover,Defender 110;
Cars,Land Rover,Defender 90;
Cars,Land Rover,Defender Ice Edition;
Cars,Land Rover,Defender;
Cars,Land Rover,Discovery Series II;
Cars,Land Rover,Discovery;
Cars,Land Rover,Freelander;
Cars,Land Rover,LR2;
Cars,Land Rover,LR3;
Cars,Land Rover,LR4;
Cars,Land Rover,Range Rover Classic;
Cars,Land Rover,Range Rover Evoque;
Cars,Land Rover,Range Rover Sport;
Cars,Land Rover,Range Rover;
Cars,Land Rover,Sterling;
Cars,Lexus,CT;
Cars,Lexus,ES;
Cars,Lexus,GS;
Cars,Lexus,GX;
Cars,Lexus,HS;
Cars,Lexus,IS F;
Cars,Lexus,IS;
Cars,Lexus,IS-F;
Cars,Lexus,LFA;
Cars,Lexus,LS Hybrid;
Cars,Lexus,LS;
Cars,Lexus,LX;
Cars,Lexus,RX Hybrid;
Cars,Lexus,RX;
Cars,Lexus,SC;
Cars,Lincoln,Aviator;
Cars,Lincoln,Blackwood;
Cars,Lincoln,Continental Mark VII;
Cars,Lincoln,Continental;
Cars,Lincoln,LS;
Cars,Lincoln,Mark LT;
Cars,Lincoln,Mark VII;
Cars,Lincoln,Mark VIII;
Cars,Lincoln,MKS;
Cars,Lincoln,MKT;
Cars,Lincoln,MKX;
Cars,Lincoln,MKZ;
Cars,Lincoln,Navigator L;
Cars,Lincoln,Navigator;
Cars,Lincoln,Town Car;
Cars,Lincoln,Zephyr;
Cars,Lotus,Elan;
Cars,Lotus,Elise;
Cars,Lotus,Esprit Turbo;
Cars,Lotus,Esprit;
Cars,Lotus,Evora;
Cars,Lotus,Exige;
Cars,Maserati,228;
Cars,Maserati,430;
Cars,Maserati,Biturbo;
Cars,Maserati,Coupe;
Cars,Maserati,Gran Sport;
Cars,Maserati,GranSport;
Cars,Maserati,GranTurismo;
Cars,Maserati,Karif;
Cars,Maserati,Quattroporte;
Cars,Maserati,Spyder;
Cars,Maybach,57;
Cars,Maybach,57S;
Cars,Maybach,62;
Cars,Maybach,Landaulet;
Cars,Mazda,323;
Cars,Mazda,626;
Cars,Mazda,929;
Cars,Mazda,B2000;
Cars,Mazda,B2500;
Cars,Mazda,B2600;
Cars,Mazda,B-Series Plus;
Cars,Mazda,B-Series;
Cars,Mazda,CX-5;
Cars,Mazda,CX-7;
Cars,Mazda,CX-9;
Cars,Mazda,Familia;
Cars,Mazda,GLC;
Cars,Mazda,Mazda2;
Cars,Mazda,Mazda3;
Cars,Mazda,Mazda5;
Cars,Mazda,Mazda6 5-Door;
Cars,Mazda,Mazda6 Sport;
Cars,Mazda,Mazda6;
Cars,Mazda,Mazdaspeed 3;
Cars,Mazda,Mazdaspeed6;
Cars,Mazda,Miata MX-5;
Cars,Mazda,Millenia;
Cars,Mazda,MPV;
Cars,Mazda,MX-3;
Cars,Mazda,MX-5;
Cars,Mazda,MX-6;
Cars,Mazda,Navajo;
Cars,Mazda,Protege;
Cars,Mazda,Protege5;
Cars,Mazda,RX-7;
Cars,Mazda,RX-8;
Cars,Mazda,Tribute;
Cars,McLaren,MP4-12C;
Cars,Mercedes-Benz,190E;
Cars,Mercedes-Benz,300CE;
Cars,Mercedes-Benz,300D;
Cars,Mercedes-Benz,300E;
Cars,Mercedes-Benz,300SD;
Cars,Mercedes-Benz,300SE;
Cars,Mercedes-Benz,300SL;
Cars,Mercedes-Benz,300TE;
Cars,Mercedes-Benz,400E;
Cars,Mercedes-Benz,400SE;
Cars,Mercedes-Benz,400SEL;
Cars,Mercedes-Benz,500E;
Cars,Mercedes-Benz,500SEC;
Cars,Mercedes-Benz,500SEL;
Cars,Mercedes-Benz,500SL;
Cars,Mercedes-Benz,600SEC;
Cars,Mercedes-Benz,600SEL;
Cars,Mercedes-Benz,600SL;
Cars,Mercedes-Benz,C-Class;
Cars,Mercedes-Benz,CL65 AMG;
Cars,Mercedes-Benz,CL-Class;
Cars,Mercedes-Benz,CLK-Class;
Cars,Mercedes-Benz,CLS-Class;
Cars,Mercedes-Benz,E-Class;
Cars,Mercedes-Benz,G55 AMG;
Cars,Mercedes-Benz,G-Class;
Cars,Mercedes-Benz,GL-Class;
Cars,Mercedes-Benz,GLK-Class;
Cars,Mercedes-Benz,M-Class;
Cars,Mercedes-Benz,R-Class;
Cars,Mercedes-Benz,S-Class;
Cars,Mercedes-Benz,SL65 AMG;
Cars,Mercedes-Benz,SL-Class;
Cars,Mercedes-Benz,SLK55 AMG;
Cars,Mercedes-Benz,SLK-Class;
Cars,Mercedes-Benz,SLR McLaren;
Cars,Mercedes-Benz,SLS AMG;
Cars,Mercedes-Benz,SLS-Class;
Cars,Mercedes-Benz,Sprinter 2500;
Cars,Mercedes-Benz,Sprinter 3500;
Cars,Mercedes-Benz,Sprinter;
Cars,Mercedes-Benz,W123;
Cars,Mercedes-Benz,W126;
Cars,Mercedes-Benz,W201;
Cars,Mercury,Capri;
Cars,Mercury,Cougar;
Cars,Mercury,Grand Marquis;
Cars,Mercury,Lynx;
Cars,Mercury,Marauder;
Cars,Mercury,Mariner;
Cars,Mercury,Marquis;
Cars,Mercury,Milan;
Cars,Mercury,Montego;
Cars,Mercury,Monterey;
Cars,Mercury,Mountaineer;
Cars,Mercury,Mystique;
Cars,Mercury,Sable;
Cars,Mercury,Topaz;
Cars,Mercury,Tracer;
Cars,Mercury,Villager;
Cars,Merkur,XR4Ti;
Cars,MG,MGB;
Cars,MINI,Clubman;
Cars,MINI,Cooper Clubman;
Cars,MINI,Cooper Countryman;
Cars,MINI,Cooper;
Cars,MINI,Countryman;
Cars,MINI,MINI;
Cars,Mitsubishi,3000GT;
Cars,Mitsubishi,Challenger;
Cars,Mitsubishi,Chariot;
Cars,Mitsubishi,Cordia;
Cars,Mitsubishi,Diamante;
Cars,Mitsubishi,Eclipse;
Cars,Mitsubishi,Endeavor;
Cars,Mitsubishi,Excel;
Cars,Mitsubishi,Expo;
Cars,Mitsubishi,Galant;
Cars,Mitsubishi,GTO;
Cars,Mitsubishi,i-MiEV;
Cars,Mitsubishi,L300;
Cars,Mitsubishi,Lancer Evolution;
Cars,Mitsubishi,Lancer;
Cars,Mitsubishi,Mighty Max Macro;
Cars,Mitsubishi,Mighty Max;
Cars,Mitsubishi,Mirage;
Cars,Mitsubishi,Montero Sport;
Cars,Mitsubishi,Montero;
Cars,Mitsubishi,Outlander Sport;
Cars,Mitsubishi,Outlander;
Cars,Mitsubishi,Pajero;
Cars,Mitsubishi,Precis;
Cars,Mitsubishi,Raider;
Cars,Mitsubishi,RVR;
Cars,Mitsubishi,Sigma;
Cars,Mitsubishi,Space;
Cars,Mitsubishi,Starion;
Cars,Mitsubishi,Tredia;
Cars,Mitsubishi,Truck;
Cars,Mitsubishi,Tundra;
Cars,Morgan,Aero 8;
Cars,Nissan,200SX;
Cars,Nissan,240SX;
Cars,Nissan,280ZX;
Cars,Nissan,300ZX;
Cars,Nissan,350Z Roadster;
Cars,Nissan,350Z;
Cars,Nissan,370Z;
Cars,Nissan,Altima;
Cars,Nissan,Armada;
Cars,Nissan,cube;
Cars,Nissan,Datsun/Nissan Z-car;
Cars,Nissan,Frontier;
Cars,Nissan,GT-R;
Cars,Nissan,JUKE;
Cars,Nissan,Leaf;
Cars,Nissan,Maxima;
Cars,Nissan,Murano;
Cars,Nissan,NV1500;
Cars,Nissan,NV2500;
Cars,Nissan,NV3500;
Cars,Nissan,NX;
Cars,Nissan,Pathfinder Armada;
Cars,Nissan,Pathfinder;
Cars,Nissan,Quest;
Cars,Nissan,Rogue;
Cars,Nissan,Sentra;
Cars,Nissan,Stanza;
Cars,Nissan,Titan;
Cars,Nissan,Versa;
Cars,Nissan,Xterra;
Cars,Oldsmobile,88;
Cars,Oldsmobile,98;
Cars,Oldsmobile,Achieva;
Cars,Oldsmobile,Alero;
Cars,Oldsmobile,Aurora;
Cars,Oldsmobile,Bravada;
Cars,Oldsmobile,Ciera;
Cars,Oldsmobile,Custom Cruiser;
Cars,Oldsmobile,Cutlass Cruiser;
Cars,Oldsmobile,Cutlass Supreme;
Cars,Oldsmobile,Cutlass;
Cars,Oldsmobile,Intrigue;
Cars,Oldsmobile,LSS;
Cars,Oldsmobile,Silhouette;
Cars,Oldsmobile,Toronado;
Cars,Panoz,Esperante;
Cars,Peugeot,106 ;
Cars,Peugeot,107 ;
Cars,Peugeot,205 ;
Cars,Peugeot,206 ;
Cars,Peugeot,206+;
Cars,Peugeot,207 ;
Cars,Peugeot,208 ;
Cars,Peugeot,301 ;
Cars,Peugeot,306 ;
Cars,Peugeot,307 ;
Cars,Peugeot,308 ;
Cars,Peugeot,405 ;
Cars,Peugeot,406 ;
Cars,Peugeot,407 ;
Cars,Peugeot,508 ;
Cars,Peugeot,605 ;
Cars,Peugeot,607 ;
Cars,Peugeot,806 ;
Cars,Peugeot,807 ;
Cars,Peugeot,1007;
Cars,Peugeot,RCZ;
Cars,Plymouth,Acclaim;
Cars,Plymouth,Breeze;
Cars,Plymouth,Colt Vista;
Cars,Plymouth,Colt;
Cars,Plymouth,Fury;
Cars,Plymouth,Grand Voyager;
Cars,Plymouth,Horizon;
Cars,Plymouth,Laser;
Cars,Plymouth,Neon;
Cars,Plymouth,Prowler;
Cars,Plymouth,Reliant;
Cars,Plymouth,Roadrunner;
Cars,Plymouth,Sundance;
Cars,Plymouth,Volare;
Cars,Plymouth,Voyager;
Cars,Pontiac,1000;
Cars,Pontiac,6000;
Cars,Pontiac,Aztek;
Cars,Pontiac,Bonneville;
Cars,Pontiac,Chevette;
Cars,Pontiac,Daewoo Kalos;
Cars,Pontiac,Fiero;
Cars,Pontiac,Firebird Formula;
Cars,Pontiac,Firebird Trans Am;
Cars,Pontiac,Firebird;
Cars,Pontiac,Firefly;
Cars,Pontiac,G3;
Cars,Pontiac,G5;
Cars,Pontiac,G6;
Cars,Pontiac,G8;
Cars,Pontiac,Gemini;
Cars,Pontiac,Grand Am;
Cars,Pontiac,Grand Prix Turbo;
Cars,Pontiac,Grand Prix;
Cars,Pontiac,GTO;
Cars,Pontiac,Lemans;
Cars,Pontiac,Montana SV6;
Cars,Pontiac,Montana;
Cars,Pontiac,Monterey;
Cars,Pontiac,Parisienne;
Cars,Pontiac,Safari;
Cars,Pontiac,Solstice;
Cars,Pontiac,Sunbird;
Cars,Pontiac,Sunfire;
Cars,Pontiac,Tempest;
Cars,Pontiac,Torrent;
Cars,Pontiac,Trans Sport;
Cars,Pontiac,Turbo Firefly;
Cars,Pontiac,Vibe;
Cars,Porsche,911;
Cars,Porsche,914;
Cars,Porsche,924 S;
Cars,Porsche,924;
Cars,Porsche,928;
Cars,Porsche,944;
Cars,Porsche,968;
Cars,Porsche,Boxster;
Cars,Porsche,Carrera GT;
Cars,Porsche,Cayenne;
Cars,Porsche,Cayman;
Cars,Porsche,Panamera;
Cars,Ram,1500;
Cars,Ram,2500;
Cars,Ram,3500;
Cars,Ram,C/V;
Cars,Ram,Dakota;
Cars,Rambler,Classic;
Cars,Renault,Alliance;
Cars,Rolls-Royce,Ghost;
Cars,Rolls-Royce,Phantom;
Cars,Saab,900;
Cars,Saab,9000;
Cars,Saab,9-2X;
Cars,Saab,9-3;
Cars,Saab,9-4X;
Cars,Saab,9-5;
Cars,Saab,9-7X;
Cars,Saturn,Astra;
Cars,Saturn,Aura;
Cars,Saturn,Ion;
Cars,Saturn,L-Series;
Cars,Saturn,Outlook;
Cars,Saturn,Relay;
Cars,Saturn,Sky;
Cars,Saturn,S-Series;
Cars,Saturn,VUE;
Cars,Scion,FR-S;
Cars,Scion,iQ;
Cars,Scion,tC;
Cars,Scion,xA;
Cars,Scion,xB;
Cars,Scion,xD;
Cars,Shelby,GT350;
Cars,Shelby,GT500;
Cars,Smart,Fortwo;
Cars,Spyker Cars,C8;
Cars,Spyker,C8 Double 12 S;
Cars,Spyker,C8 Laviolette;
Cars,Spyker,C8 Spyder Wide Body;
Cars,Spyker,C8 Spyder;
Cars,Studebaker,Avanti;
Cars,Subaru,Alcyone SVX;
Cars,Subaru,B9 Tribeca;
Cars,Subaru,Baja;
Cars,Subaru,Brat;
Cars,Subaru,BRZ;
Cars,Subaru,Forester;
Cars,Subaru,Impreza WRX;
Cars,Subaru,Impreza;
Cars,Subaru,Justy;
Cars,Subaru,Legacy;
Cars,Subaru,Leone;
Cars,Subaru,Loyale;
Cars,Subaru,Outback Sport;
Cars,Subaru,Outback;
Cars,Subaru,SVX;
Cars,Subaru,Tribeca;
Cars,Subaru,XT;
Cars,Suzuki,Aerio;
Cars,Suzuki,Cultus;
Cars,Suzuki,Daewoo Lacetti;
Cars,Suzuki,Daewoo Magnus;
Cars,Suzuki,Equator;
Cars,Suzuki,Esteem;
Cars,Suzuki,Forenza;
Cars,Suzuki,Grand Vitara;
Cars,Suzuki,Kizashi;
Cars,Suzuki,Reno;
Cars,Suzuki,Samurai;
Cars,Suzuki,Sidekick;
Cars,Suzuki,SJ 410;
Cars,Suzuki,SJ;
Cars,Suzuki,Swift;
Cars,Suzuki,SX4;
Cars,Suzuki,Verona;
Cars,Suzuki,Vitara;
Cars,Suzuki,XL7;
Cars,Suzuki,XL-7;
Cars,Tesla,Model S;
Cars,Tesla,Roadster;
Cars,Toyota,4Runner;
Cars,Toyota,Avalon;
Cars,Toyota,Camry Hybrid;
Cars,Toyota,Camry Solara;
Cars,Toyota,Camry;
Cars,Toyota,Celica;
Cars,Toyota,Corolla;
Cars,Toyota,Cressida;
Cars,Toyota,Echo;
Cars,Toyota,FJ Cruiser;
Cars,Toyota,Highlander Hybrid;
Cars,Toyota,Highlander;
Cars,Toyota,Ipsum;
Cars,Toyota,Land Cruiser;
Cars,Toyota,Matrix;
Cars,Toyota,MR2;
Cars,Toyota,Paseo;
Cars,Toyota,Previa;
Cars,Toyota,Prius c;
Cars,Toyota,Prius Plug-in Hybrid;
Cars,Toyota,Prius Plug-in;
Cars,Toyota,Prius v;
Cars,Toyota,Prius;
Cars,Toyota,RAV4;
Cars,Toyota,Sequoia;
Cars,Toyota,Sienna;
Cars,Toyota,Solara;
Cars,Toyota,Supra;
Cars,Toyota,T100;
Cars,Toyota,Tacoma Xtra;
Cars,Toyota,Tacoma;
Cars,Toyota,Tercel;
Cars,Toyota,Truck Xtracab SR5;
Cars,Toyota,Tundra;
Cars,Toyota,TundraMax;
Cars,Toyota,Venza;
Cars,Toyota,Xtra;
Cars,Toyota,Yaris;
Cars,Volkswagen,Beetle;
Cars,Volkswagen,Cabriolet;
Cars,Volkswagen,CC;
Cars,Volkswagen,Corrado;
Cars,Volkswagen,Eos;
Cars,Volkswagen,Eurovan;
Cars,Volkswagen,Fox;
Cars,Volkswagen,GLI;
Cars,Volkswagen,Golf III;
Cars,Volkswagen,Golf;
Cars,Volkswagen,GTI;
Cars,Volkswagen,Jetta III;
Cars,Volkswagen,Jetta;
Cars,Volkswagen,New Beetle;
Cars,Volkswagen,Passat;
Cars,Volkswagen,Phaeton;
Cars,Volkswagen,Quantum;
Cars,Volkswagen,R32;
Cars,Volkswagen,Rabbit;
Cars,Volkswagen,rio;
Cars,Volkswagen,riolet;
Cars,Volkswagen,Routan;
Cars,Volkswagen,Scirocco;
Cars,Volkswagen,Tiguan;
Cars,Volkswagen,Touareg 2;
Cars,Volkswagen,Touareg;
Cars,Volkswagen,Type 2;
Cars,Volkswagen,Vanagon;
Minivan,Volkswagen-Minivan,Caddy;
Minivan,Volkswagen-Minivan,Caravelle;
Minivan,Volkswagen-Minivan,Crafter;
Minivan,Volkswagen-Minivan,Multivan;
Minivan,Volkswagen-Minivan,Transporter;
Minivan,Volkswagen,LT;
Suv,Volkswagen-Suv,Amarok;
Suv,Volkswagen-Suv,Tiguan;
Suv,Volkswagen-Suv,Tiguan AllSpace;
Suv,Volkswagen-Suv,Touareg;
Suv,Volkswagen-Suv,T - Roc;
Cars,Volvo,240;
Cars,Volvo,740;
Cars,Volvo,850;
Cars,Volvo,940;
Cars,Volvo,960;
Cars,Volvo,C30;
Cars,Volvo,C70;
Cars,Volvo,S40;
Cars,Volvo,S60;
Cars,Volvo,S70;
Cars,Volvo,S80;
Cars,Volvo,V40;
Cars,Volvo,V50;
Cars,Volvo,V70;
Cars,Volvo,XC60;
Cars,Volvo,XC70;
Cars,Volvo,XC90;";

    }

}