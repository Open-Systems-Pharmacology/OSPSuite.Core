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

task :create_local_nuget do
  versionId = "12.0.0-" + generate_code(5)
  puts("Your version is " + versionId)
  exec("dotnet", "pack", "-p:PackageVersion="+ versionId, "--configuration", "Debug", "--output", "nuget_repo", "--no-build") 
end

private
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
  