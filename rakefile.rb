require_relative 'scripts/coverage'
require_relative 'scripts/utils'
require_relative 'scripts/copy-dependencies'

task :cover do 
  filter = []
  filter << "+[OSPSuite.Core]*"
  filter << "+[OSPSuite.Infrastructure]*"
  filter << "+[OSPSuite.Presentation]*"
  filter << "+[OSPSuite.R]*"
  
  #exclude namespaces that are tested from applications
  filter << "-[OSPSuite.Infrastructure.Serialization]OSPSuite.Infrastructure.Serialization.ORM*"
  filter << "-[OSPSuite.Presentation]OSPSuite.Presentation.MenuAndBars*"
  filter << "-[OSPSuite.Presentation]OSPSuite.Presentation.Presenters.ContextMenus*"
  
  
  targetProjects = [
	"OSPSuite.Core.Tests.csproj",
	"OSPSuite.Core.IntegrationTests.csproj",
	"OSPSuite.Infrastructure.Tests.csproj",
	"OSPSuite.Presentation.Tests.csproj",
	"OSPSuite.R.Tests.csproj",
	];
	
	Coverage.cover(filter, targetProjects)
end
  
task :copy_to_pksim do
  copy_to_app '../PK-Sim/src/PKSim/bin/Debug/net472'
end

task :copy_to_mobi do
  copy_to_app '../MoBi/src/MoBi/bin/Debug/net472'
end

task :create_local_nuget, [:arg1, :arg2, :arg3] do |t, args|
  FileUtils.rm_f Dir.glob("./nuget_repo/*.nupkg")
  versionId = "12.0.0-" + generate_code(5)
  puts("Your version is " + versionId.red)
  system("dotnet", "pack", "-p:PackageVersion="+ versionId, "--configuration", "Debug", "--output", "nuget_repo", "--no-build") 
  if args.to_hash.values.include? "m"
    update_mobi(versionId)
  end
  if args.to_hash.values.include? "p"
    update_pksim(versionId)
  end
  if args.to_hash.values.include? "r"
    update_ospsuite_r(versionId)
  end
  

end

private
def find_token(file, regex)
  file_content = str = IO.read(file)
  matches = file_content.match(regex)

  if(matches.nil?)
    return nil
  end
  return matches[1]
end

def update_ospsuite_r(versionId)
  puts("updating OSPSuite-R")
  token = find_token("../OSPSuite-R/packages.config", /<package id="OSPSuite.Core" version="(.*)"/)
  if(token.nil?)
    return
  end
  
  Utils.replace_tokens({token => versionId}, "../OSPSuite-R/packages.config")
end

def update_mobi(versionId)
  puts("updating MoBi")
  token = find_token("../MoBi/src/MoBi/MoBi.csproj", /<PackageReference Include="OSPSuite.Core" Version="(.*)"/)
  if(token.nil?)
    return
  end

  glob = Dir.glob('../MoBi/**/*.csproj')
  glob.each do |file|
    Utils.replace_tokens({token => versionId}, file)
  end

end

def update_pksim(versionId)
  puts("updating PKSim")
  token = find_token("../PK-Sim/src/PKSim/PKSim.csproj", /<PackageReference Include="OSPSuite.Core" Version="(.*)"/)
  if(token.nil?)
    return
  end

  glob = Dir.glob('../PK-Sim/**/*.csproj')
  glob.each do |file|
    Utils.replace_tokens({token => versionId}, file)
  end
end

def generate_code(number)
  charset = Array('A'..'Z') + Array('a'..'z')
  Array.new(number) { charset.sample }.join
end

def copy_to_app(app_target_relative_path)
  app_target_path = File.join(solution_dir, app_target_relative_path)
  source_dir = File.join(tests_dir, 'OSPSuite.Starter', 'bin', 'Debug', "net472")

  copy_dependencies source_dir,  app_target_path do
    copy_file 'OSPSuite.*.dll'
    copy_file 'OSPSuite.*.pdb'
end

end

def solution_dir
  File.dirname(__FILE__)
end

def tests_dir
  File.join(solution_dir, 'tests')
end
  