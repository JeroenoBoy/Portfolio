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
    }
  );
}
