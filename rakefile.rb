require_relative 'scripts/coverage'
require_relative 'scripts/copy-dependencies'

task :cover do 
  filter = []
  filter << "+[OSPSuite.Core]*"
  filter << "+[OSPSuite.Infrastructure]*"
  filter << "+[OSPSuite.Presentation]*"
  filter << "+[OSPSuite.Engine]*"

  #exclude namespaces that are tested from applications
  filter << "-[OSPSuite.Infrastructure]OSPSuite.Infrastructure.Reporting*"
  filter << "-[OSPSuite.Infrastructure]OSPSuite.Infrastructure.Serialization.ORM*"
  filter << "-[OSPSuite.Presentation]OSPSuite.Presentation.MenuAndBars*"
  filter << "-[OSPSuite.Presentation]OSPSuite.Presentation.Presenters.ContextMenus*"

  Coverage.cover(filter, "OSPSuite.Core.Tests.csproj")
end

task :copy_to_pksim do
	copy_to_app '../PK-Sim/src/PKSim/bin/Debug/'
end

task :copy_to_mobi do
	copy_to_app '../MoBi/src/MoBi/bin/Debug/'
end

private

def copy_to_app(app_target_relative_path)
  app_target_path = File.join(solution_dir, app_target_relative_path)
  source_dir = File.join(tests_dir, 'OSPSuite.Starter', 'bin', 'Debug')

  copy_depdencies source_dir,  app_target_path do
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
	