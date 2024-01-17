% Define dynamic predicate for grapes
:- dynamic grape/7.
grape(cabernet_sauvignon, red, 13, 1, 3, 3, 3).
grape(chardonnay, white, 13, 1, 3, 3, 1).
grape(pinot_noir, red, 13, 1, 3, 3, 2).
grape(merlot, red, 13, 1, 3, 3, 3).
grape(syrah_shiraz, red, 14, 1, 3, 3, 3).
grape(sangiovese, red, 13, 1, 3, 3, 3).
grape(sauvignon_blanc, white, 12, 1, 3, 2, 1).
grape(tempranillo, red, 13, 1, 3, 3, 3).
grape(blend, red, 13, 1, 2, 3, 2).
grape(grenache, red, 13, 1, 3, 3, 3).
grape(riesling, white, 10, 2, 3, 2, 1).
grape(nebbiolo, red, 14, 1, 3, 3, 4).
grape(moscato, white, 6, 3, 1, 2, 1).
grape(malbec, red, 13, 1, 3, 4, 3).
grape(carmenere, red, 13, 1, 3, 3, 3).
grape(zinfandel, red, 13, 1, 2, 3, 2).
grape(barbera, red, 13, 1, 3, 3, 2).
grape(semillon, white, 12, 2, 3, 3, 1).
grape(montepulciano, red, 13, 1, 3, 3, 3).
grape(corvina, red, 14, 1, 3, 4, 3).
grape(monastrell, red, 13, 1, 2, 3, 3).
grape(garnacha, red, 13, 1, 3, 3, 2).
grape(macabeo, white, 11, 1, 3, 2, 1).
grape(cabernet_franc, red, 13, 1, 3, 3, 3).
grape(viognier, white, 13, 1, 3, 3, 1).
grape(primitivo, red, 14, 1, 2, 4, 3).
grape(nero_d_avola, red, 13, 1, 3, 3, 3).
grape(gamay, red, 12, 1, 3, 2, 2).
grape(trebbiano, white, 12, 1, 3, 2, 1).
grape(pinot_grigio, white, 12, 1, 3, 2, 1).
grape(glera, white, 11, 1, 3, 2, 1).
grape(gewurztraminer, white, 13, 2, 3, 3, 1).
grape(carignan, red, 13, 1, 3, 3, 3).
grape(pinot_gris, white, 13, 1, 3, 2, 1).
grape(chenin_blanc, white, 12, 1, 3, 2, 1).
grape(garganega, white, 12, 1, 3, 2, 1).
grape(pinot_meunier, red, 12, 1, 4, 3, 1).
grape(touriga_nacional, red, 16, 2, 2, 4, 2).
grape(aglianico, red, 13, 1, 2, 4, 3).
grape(vermentino, white, 12, 1, 3, 2, 1).
grape(grenache_blanc, red, 13, 1, 3, 2, 1).
grape(sangiovese_grosso, red, 14, 1, 3, 3, 3).
grape(dolcetto, red, 13, 1, 3, 3, 2).
grape(airen, white, 10, 2, 3, 2, 1).
grape(bordeaux_blend_red, red, 13, 1, 3, 3, 3).
grape(negroamaro, red, 13, 1, 3, 3, 3).
grape(malvasia, white, 11, 2, 2, 3, 1).
grape(cortese, white, 12, 1, 3, 2, 1).
grape(prosecco, white, 16, 1, 2, 3, 1).
grape(brachetto, red, 5, 4, 1, 2, 1).

													

% Add a new grape

add_grape(Name, Color, Alkol, Sugar, Acidity, Body, Tannin) :-
    assert(grape(Name, Color, Alkol, Sugar, Acidity, Body, Tannin)).

% Remove a grape
remove_grape(Name) :-
    retract(grape(Name, _, _, _, _, _, _)).

													

g_quality(X,Type,Q) :-
    grape(X,Type,Abv,S,A,_,_),
    quality_abv(Type, Abv, Q1),
    quality_s(Type,S, Q2),
    quality_a(Type,A, Q3),
    SumQ is (Q1 + Q2 + Q3),
    calculate_last(SumQ,Type,Q).

% Alkol oranı kalitesi
quality_abv(red, Abv, Quality) :- Quality is Abv * 10.
quality_abv(white, Abv, Quality) :- Quality is Abv * 8.

% Tatlılık kalitesi
quality_s(red, S, Quality) :- Quality is S * 1.
quality_s(white,S, Quality) :- Quality is S * 10.

% Asitlik kalitesi
quality_a(red,A, Quality) :- Quality is A * (-2).
quality_a(white,A, Quality) :- Quality is A * (-6).

calculate_last(SumQ,red,Q):-
TempSumQ =SumQ-41,
Q is round((10 * TempSumQ) / 122).

calculate_last(SumQ,white,Q):-
TempSumQ =SumQ-20,
Q is round((10 * TempSumQ) / 152).

													

% Özel yuvarlama fonksiyonu
custom_round(Value, DecimalPlaces, RoundedValue) :-
    Multiplier is 10 ** DecimalPlaces,
    TempValue is Value * Multiplier,
    RoundedValue is floor(TempValue + 0.5) / Multiplier.



distribute_remaining(_, [], [], UpdatedGrapenames, UpdatedTon, [], []).

distribute_remaining(RemainingTon, [Grapename|GrapenameRest], [Ton|TonRest], UpdatedGrapenames, UpdatedTon,
    Recipes_N, Recipes_T) :-
    Add_Ton is Ton - RemainingTon,
    Add_Ton > 0,
    UpdatedTon = [Add_Ton | TonRest],
    UpdatedGrapenames = [Grapename | GrapenameRest],
    Recipes_N = [Grapename],
    Recipes_T = [RemainingTon].

distribute_remaining(RemainingTon, [Grapename|GrapenameRest], [Ton|TonRest], UpdatedGrapenames, UpdatedTon,
    Recipes_N, Recipes_T) :-
    Add_Ton is Ton - RemainingTon,
    Add_Ton = 0,
    UpdatedGrapenames = [GrapenameRest],
    UpdatedTon = [TonRest], 
    Recipes_N = [Grapename],
    Recipes_T = [Ton].

distribute_remaining(RemainingTon, [Grapename|GrapenameRest], [Ton|TonRest], UpdatedGrapenames, UpdatedTon,
    Recipes_N, Recipes_T):-
    Remaining is RemainingTon - Ton,
    distribute_remaining(Remaining, GrapenameRest, TonRest, UpdatedGrapenames, UpdatedTon,
        Result_N, Result_P),
    Recipes_N = [Grapename|Result_N],
    Recipes_T = [Ton|Result_P].

													
make_recipes_helper([],[],[],[],[]).

make_recipes_helper([Grapename|GrapenameRest], [Ton|TonRest], [Recipes_N|RestRecipes_N], [Recipes_T|RestRecipes_T], [Total|RestTotal]) :-
    NewTon is min(Ton, 1.5),
    RemainingTon is 1.5 - NewTon,

    (RemainingTon =:= 0 ->
        Total is round((Ton / 1.5) - 0.5),
        Rest is (Ton - (Total * 1.5)),
        Recipes_N = [Grapename],
        Recipes_T = [1.5],
        (Rest = 0 ->
            UpdatedGrapenames = GrapenameRest,
            UpdatedPercents = TonRest
        ;
            UpdatedGrapenames = [Grapename|GrapenameRest],
            UpdatedPercents = [Rest|TonRest]
        )
    ;
        distribute_remaining(RemainingTon, GrapenameRest, TonRest, UpdatedGrapenames, UpdatedPercents,
                              Recipes_NT, Recipes_TT),
        Recipes_N = [Grapename|Recipes_NT],
        Recipes_T = [Ton|Recipes_TT],
        Total = 1
    ),
    make_recipes_helper(UpdatedGrapenames, UpdatedPercents, RestRecipes_N, RestRecipes_T, RestTotal).

													


calculate_quality([Grape|[]], [Ton|[]], RoundedQuality) :-
    g_quality(Grape, _, Q),
    Quality is Q * Ton / 1.5,
    RoundedQuality is round(Quality).
 
calculate_quality([Grape|Rest_Grape], [Ton|Rest_Ton], RoundedQuality) :-
    calculate_quality(Rest_Grape, Rest_Ton, Rest_Quality),
    g_quality(Grape, _, Q),
    Quality is Q * Ton / 1.5 + Rest_Quality,
    RoundedQuality is round(Quality).

													

calculate_quality_helper([],[],[]).

calculate_quality_helper([Grapes|RestGrapes],[Percents|RestPercents],[Quality|RestQuality]):-
    calculate_quality(Grapes,Percents,Quality),
    calculate_quality_helper(RestGrapes,RestPercents,RestQuality).

													

make_recipes(Grapenames, Tons, RecipesN,RecipesT,Qualitys,Total) :-

    make_recipes_helper(Grapenames, Tons, RecipesN,RecipesT,Total),
    calculate_quality_helper(RecipesN,RecipesT,Qualitys).


aging(Grapes, Tons, Year) :-
    aging_helper(Grapes, Tons, SumTannin),
    Tannin is round(SumTannin / 1.5),
    (Tannin = 1 -> Year is 1; Year is Tannin * 5).

aging_helper([], [], 0).

aging_helper([Grape | RestGrapes], [Ton | RestTon], SumTannin) :-
    aging_helper(RestGrapes, RestTon, SubTannin),
    grape(Grape, _, _, _, _, _, Tannin),
    SumTannin is SubTannin + (Tannin * Ton).


