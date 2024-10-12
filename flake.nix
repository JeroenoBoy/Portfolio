{
  description = "Portfolio flake";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
    utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, utils }: utils.lib.eachDefaultSystem (system: 
    let
      pkgs = (import nixpkgs) { inherit system; };
    in {
      devShell = pkgs.mkShell {
        name = "Portfolio flake";
        buildInputs = with pkgs; [
          nodejs
        ];

        shellHook = ''
          npm install
          echo ""
          echo "Node : `node --version`"
          echo "Npm : `npm --version`"
          echo "
          Welcome to my Portfolio shell 🚀!
          ";
        '';
      };

      packages = rec {
        default = portfolio;
        portfolio = pkgs.buildNpmPackage {
          buildInputs =  with pkgs; [ nodejs ];
          name = "Portfolio";
          src = ./.;

          npmDepsHash = "sha256-9J30U+i/T8p6XS+vl4yX4jge6HzkMBZIrPeg/95FkeI=";
          npmBuild = "npm ci;npm run build";

          installPhase = ''
            echo "Copying files"
            mkdir -p $out/bin
            cp -r public $out/
            cp -r .next $out/
            cp -r next.config.js $out/
            cp -r node_modules $out/
            cp package.json $out/

            echo "#!/bin/sh
            cd $out
            ${pkgs.nodejs_22}/bin/npm run start
            " &> $out/bin/Portfolio

            echo "Changing permissions"
            chmod +x $out/bin/Portfolio
          '';
        };
      };
    }
  );
}
